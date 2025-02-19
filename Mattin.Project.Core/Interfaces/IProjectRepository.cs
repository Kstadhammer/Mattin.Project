using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<Result<ProjectEntity?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<string>> GenerateProjectNumberAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProjectEntity>>> GetProjectsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken = default
    );
    Task<Result> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
