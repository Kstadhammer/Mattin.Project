using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Interfaces;

public interface IStatusRepository : IBaseRepository<Status>
{
    Task<Result<Status?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<Status?>> GetDefaultStatusAsync(CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
