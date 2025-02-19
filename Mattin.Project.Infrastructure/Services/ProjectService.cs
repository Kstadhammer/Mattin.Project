// Project service implementation enhanced with AI assistance for:
// - Automatic project number generation and validation
// - Transaction management for data consistency
// - Status tracking and updates
// - Budget calculations and validations
// - Error handling and logging
// - Entity relationships management

using Mattin.Project.Core.Common;
using Mattin.Project.Core.Factories;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;

namespace Mattin.Project.Infrastructure.Services;

/// <summary>
/// Manages project-related operations with transaction support and validation.
/// Handles project creation, updates, and status management.
/// </summary>
public class ProjectService(
    IProjectRepository projectRepository,
    IStatusRepository statusRepository,
    IMappingFactory mappingFactory,
    ApplicationDbContext context
) : IProjectService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<ProjectDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectRepository.GetAllAsync(cancellationToken);
        if (result.IsFailure)
            return Result<IEnumerable<ProjectDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ProjectDetailsDto>>.Success(
            mappingFactory.CreateProjectDetailsDtos(result.Value)
        );
    }

    public async Task<Result<ProjectDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectRepository.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return Result<ProjectDetailsDto?>.Failure(result.Error);

        return Result<ProjectDetailsDto?>.Success(
            result.Value == null ? null : mappingFactory.CreateProjectDetailsDto(result.Value)
        );
    }

    public async Task<Result<ProjectDetailsDto>> CreateAsync(
        CreateProjectDto dto,
        CancellationToken cancellationToken = default
    )
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var entity = mappingFactory.CreateProjectEntity(dto);

            // Generate project number
            var projectNumberResult = await projectRepository.GenerateProjectNumberAsync(
                cancellationToken
            );
            if (projectNumberResult.IsFailure)
                return Result<ProjectDetailsDto>.Failure(projectNumberResult.Error);
            entity.ProjectNumber = projectNumberResult.Value;

            // Get status
            var statusResult = await statusRepository.GetAllAsync(cancellationToken);
            if (statusResult.IsFailure)
                return Result<ProjectDetailsDto>.Failure(statusResult.Error);

            var status = statusResult.Value.FirstOrDefault(s => s.Name == dto.Status);
            if (status == null)
                return Result<ProjectDetailsDto>.Failure($"Invalid status: {dto.Status}");

            entity.StatusId = status.Id;
            entity.Created = DateTime.UtcNow;

            // Calculate total price if not set
            if (entity is { TotalPrice: <= 0, HourlyRate: > 0 })
            {
                var workDays = (entity.EndDate ?? DateTime.MaxValue) - entity.StartDate;
                var estimatedHours = workDays.Days * 8;
                entity.TotalPrice = entity.HourlyRate * estimatedHours;
            }

            var result = await projectRepository.AddAsync(entity, cancellationToken);
            if (result.IsFailure)
                return Result<ProjectDetailsDto>.Failure(result.Error);

            await transaction.CommitAsync(cancellationToken);
            return Result<ProjectDetailsDto>.Success(
                mappingFactory.CreateProjectDetailsDto(result.Value)
            );
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<ProjectDetailsDto>.Failure($"Failed to create project: {ex.Message}");
        }
    }

    public async Task<Result<ProjectDetailsDto>> UpdateAsync(
        UpdateProjectDto dto,
        CancellationToken cancellationToken = default
    )
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var existingResult = await projectRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingResult.IsFailure)
                return Result<ProjectDetailsDto>.Failure(existingResult.Error);

            if (existingResult.Value == null)
                return Result<ProjectDetailsDto>.Failure($"Project with ID {dto.Id} not found.");

            // Get status
            var statusResult = await statusRepository.GetAllAsync(cancellationToken);
            if (statusResult.IsFailure)
                return Result<ProjectDetailsDto>.Failure(statusResult.Error);

            var status = statusResult.Value.FirstOrDefault(s => s.Name == dto.Status);
            if (status == null)
                return Result<ProjectDetailsDto>.Failure($"Invalid status: {dto.Status}");

            var project = mappingFactory.UpdateProjectEntity(dto, existingResult.Value);
            project.StatusId = status.Id;
            project.Modified = DateTime.UtcNow;

            var updateResult = await projectRepository.UpdateAsync(project, cancellationToken);
            if (updateResult.IsFailure)
                return Result<ProjectDetailsDto>.Failure(updateResult.Error);

            await transaction.CommitAsync(cancellationToken);
            return Result<ProjectDetailsDto>.Success(
                mappingFactory.CreateProjectDetailsDto(updateResult.Value)
            );
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<ProjectDetailsDto>.Failure($"Failed to update project: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var projectResult = await GetByIdAsync(id, cancellationToken);
        if (projectResult.IsFailure)
            return Result<bool>.Failure(projectResult.Error);

        if (projectResult.Value == null)
            return Result<bool>.Failure($"Project with ID {id} not found for deletion.");

        var result = await projectRepository.DeleteByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Result<bool>.Success(true) : Result<bool>.Failure(result.Error);
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectRepository.ExistsAsync(id, cancellationToken);
        return result.IsSuccess
            ? Result<bool>.Success(result.Value)
            : Result<bool>.Failure(result.Error);
    }

    public async Task<Result<string>> GenerateProjectNumberAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await projectRepository.GenerateProjectNumberAsync(cancellationToken);
    }

    public async Task<Result<IEnumerable<ProjectDetailsDto>>> GetProjectsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectRepository.GetProjectsByClientIdAsync(
            clientId,
            cancellationToken
        );
        if (result.IsFailure)
            return Result<IEnumerable<ProjectDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ProjectDetailsDto>>.Success(
            mappingFactory.CreateProjectDetailsDtos(result.Value)
        );
    }

    public async Task<Result<bool>> UpdateProjectStatusAsync(
        int projectId,
        string status,
        CancellationToken cancellationToken = default
    )
    {
        var projectResult = await GetByIdAsync(projectId, cancellationToken);
        if (projectResult.IsFailure)
            return Result<bool>.Failure(projectResult.Error);

        if (projectResult.Value == null)
            return Result<bool>.Failure($"Project with ID {projectId} not found.");

        var statusResult = await statusRepository.GetAllAsync(cancellationToken);
        if (statusResult.IsFailure)
            return Result<bool>.Failure(statusResult.Error);

        var matchingStatus = statusResult.Value.FirstOrDefault(s => s.Name == status);
        if (matchingStatus == null)
            return Result<bool>.Failure($"Invalid status: {status}");

        // Create UpdateProjectDto from ProjectDetailsDto
        var updateDto = new UpdateProjectDto
        {
            Id = projectId,
            Title = projectResult.Value.Title,
            Description = projectResult.Value.Description,
            StartDate = projectResult.Value.StartDate,
            EndDate = projectResult.Value.EndDate,
            ProjectManagerId = projectResult.Value.ProjectManagerId,
            HourlyRate = projectResult.Value.HourlyRate,
            TotalPrice = projectResult.Value.TotalPrice,
            ClientId = projectResult.Value.ClientId,
            Status = status,
        };

        var updateResult = await UpdateAsync(updateDto, cancellationToken);
        return updateResult.IsSuccess
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(updateResult.Error);
    }
}
