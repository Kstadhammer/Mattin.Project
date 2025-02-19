// Project service implementation enhanced with AI assistance for:
// - Automatic project number generation
// - Status management
// - Price calculations
// - Error handling and validation

using AutoMapper;
using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Services.Base;

namespace Mattin.Project.Infrastructure.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    IStatusRepository statusRepository,
    IMapper mapper
) : BaseService<ProjectEntity>(projectRepository), IProjectService
{
    public async Task<IEnumerable<ProjectDetailsDto>> GetAllAsync()
    {
        var result = await projectRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return mapper.Map<IEnumerable<ProjectDetailsDto>>(result.Value);
    }

    public async Task<ProjectDetailsDto?> GetByIdAsync(int id)
    {
        var result = await projectRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        var project = result.Value.FirstOrDefault(p => p.Id == id);
        return mapper.Map<ProjectDetailsDto>(project);
    }

    public async Task<ProjectDetailsDto> CreateAsync(CreateProjectDto dto)
    {
        try
        {
            var entity = mapper.Map<ProjectEntity>(dto);
            Console.WriteLine(
                $"Debug: Mapped entity - ProjectManagerId: {entity.ProjectManagerId}, ClientId: {entity.ClientId}"
            );

            // Generate project number
            entity.ProjectNumber = await GenerateProjectNumberAsync();
            Console.WriteLine($"Debug: Generated project number: {entity.ProjectNumber}");

            // Get status by name
            var statusResult = await statusRepository.GetAllAsync();
            if (statusResult.IsFailure)
                throw new InvalidOperationException(statusResult.Error);

            var status =
                statusResult.Value.FirstOrDefault(s => s.Name == dto.Status)
                ?? throw new InvalidOperationException($"Invalid status: {dto.Status}");

            entity.StatusId = status.Id;
            entity.Created = DateTime.UtcNow;
            Console.WriteLine($"Debug: Set StatusId: {entity.StatusId}");

            // Calculate total price if not set
            if (entity is { TotalPrice: <= 0, HourlyRate: > 0 })
            {
                var workDays = (entity.EndDate ?? DateTime.MaxValue) - entity.StartDate;
                var estimatedHours = workDays.Days * 8; // Assuming 8 hours per day
                entity.TotalPrice = entity.HourlyRate * estimatedHours;
                Console.WriteLine($"Debug: Calculated total price: {entity.TotalPrice}");
            }

            Console.WriteLine(
                $"Debug: About to add entity - Title: {entity.Title}, ClientId: {entity.ClientId}, ProjectManagerId: {entity.ProjectManagerId}, StatusId: {entity.StatusId}"
            );
            var result = await projectRepository.AddAsync(entity);
            if (result.IsFailure)
                throw new InvalidOperationException(result.Error);

            return mapper.Map<ProjectDetailsDto>(result.Value);
        }
        catch (Exception ex)
        {
            var fullMessage = ex.Message;
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                fullMessage += $"\nInner Exception: {innerException.Message}";
                innerException = innerException.InnerException;
            }
            throw new InvalidOperationException(fullMessage);
        }
    }

    public async Task<ProjectDetailsDto> UpdateAsync(UpdateProjectDto dto)
    {
        try
        {
            var entity = mapper.Map<ProjectEntity>(dto);

            // Get status by name
            var statusResult = await statusRepository.GetAllAsync();
            if (statusResult.IsFailure)
                throw new InvalidOperationException(statusResult.Error);

            var status =
                statusResult.Value.FirstOrDefault(s => s.Name == dto.Status)
                ?? throw new InvalidOperationException($"Invalid status: {dto.Status}");

            entity.StatusId = status.Id;
            entity.Modified = DateTime.UtcNow;

            var result = await projectRepository.UpdateAsync(entity);
            if (result.IsFailure)
                throw new InvalidOperationException(result.Error);

            return mapper.Map<ProjectDetailsDto>(result.Value);
        }
        catch (Exception ex)
        {
            var fullMessage = ex.Message;
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                fullMessage += $"\nInner Exception: {innerException.Message}";
                innerException = innerException.InnerException;
            }
            throw new InvalidOperationException(fullMessage);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await projectRepository.DeleteAsync(id);
        return result.IsSuccess;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await projectRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value.Any(p => p.Id == id);
    }

    public async Task<string> GenerateProjectNumberAsync()
    {
        var result = await projectRepository.GenerateProjectNumberAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value;
    }

    public async Task<IEnumerable<ProjectDetailsDto>> GetProjectsByClientIdAsync(int clientId)
    {
        var result = await projectRepository.GetProjectsByClientIdAsync(clientId);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return mapper.Map<IEnumerable<ProjectDetailsDto>>(result.Value);
    }

    public async Task<bool> UpdateProjectStatusAsync(int projectId, string status)
    {
        var statusResult = await statusRepository.GetAllAsync();
        if (statusResult.IsFailure)
            throw new InvalidOperationException(statusResult.Error);

        var matchingStatus = statusResult.Value.FirstOrDefault(s => s.Name == status);
        if (matchingStatus == null)
            return false;

        var projectResult = await GetByIdAsync(projectId);
        if (projectResult == null)
            return false;

        var project = mapper.Map<ProjectEntity>(projectResult);
        project.StatusId = matchingStatus.Id;

        var updateResult = await projectRepository.UpdateAsync(project);
        return updateResult.IsSuccess;
    }
}
