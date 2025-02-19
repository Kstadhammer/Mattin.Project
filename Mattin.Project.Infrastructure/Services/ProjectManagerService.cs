// Project manager service implementation enhanced with AI assistance for:
// - Project manager data operations
// - DTO mapping and transformations
// - Error handling and validation
// - Base service functionality inheritance
// - Relationship management with projects

using Mattin.Project.Core.Common;
using Mattin.Project.Core.Factories;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.ProjectManager;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Services.Base;

namespace Mattin.Project.Infrastructure.Services;

public class ProjectManagerService(
    IProjectManagerRepository projectManagerRepository,
    IMappingFactory mappingFactory
) : BaseService<ProjectManager>(projectManagerRepository), IProjectManagerService
{
    public async Task<Result<IEnumerable<ProjectManagerDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectManagerRepository.GetAllAsync(cancellationToken);
        if (result.IsFailure)
            return Result<IEnumerable<ProjectManagerDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ProjectManagerDetailsDto>>.Success(
            mappingFactory.CreateProjectManagerDetailsDtos(result.Value)
        );
    }

    public async Task<Result<ProjectManagerDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectManagerRepository.GetAllAsync(cancellationToken);
        if (result.IsFailure)
            return Result<ProjectManagerDetailsDto?>.Failure(result.Error);

        var projectManager = result.Value.FirstOrDefault(p => p.Id == id);
        return Result<ProjectManagerDetailsDto?>.Success(
            projectManager == null
                ? null
                : mappingFactory.CreateProjectManagerDetailsDto(projectManager)
        );
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectManagerRepository.GetAllAsync(cancellationToken);
        if (result.IsFailure)
            return Result<bool>.Failure(result.Error);

        return Result<bool>.Success(result.Value.Any(p => p.Id == id));
    }
}
