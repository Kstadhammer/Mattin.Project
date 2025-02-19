using Mattin.Project.Core.Common;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Interfaces;

public interface IProjectService
{
    Task<Result<IEnumerable<ProjectDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    );
    Task<Result<ProjectDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<Result<ProjectDetailsDto>> CreateAsync(
        CreateProjectDto dto,
        CancellationToken cancellationToken = default
    );
    Task<Result<ProjectDetailsDto>> UpdateAsync(
        UpdateProjectDto dto,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<string>> GenerateProjectNumberAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProjectDetailsDto>>> GetProjectsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> UpdateProjectStatusAsync(
        int projectId,
        string status,
        CancellationToken cancellationToken = default
    );
}
