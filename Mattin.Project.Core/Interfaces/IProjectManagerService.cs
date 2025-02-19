using Mattin.Project.Core.Common;
using Mattin.Project.Core.Models.DTOs.ProjectManager;

namespace Mattin.Project.Core.Interfaces;

public interface IProjectManagerService
{
    Task<Result<IEnumerable<ProjectManagerDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    );
    Task<Result<ProjectManagerDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
