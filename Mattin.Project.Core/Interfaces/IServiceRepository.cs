using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Interfaces;

public interface IServiceRepository : IBaseRepository<Service>
{
    Task<Result<Service?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Service>>> GetServicesByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<Service>>> GetActiveServicesAsync(
        CancellationToken cancellationToken = default
    );
}
