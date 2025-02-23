using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Interfaces;

public interface IClientRepository : IBaseRepository<Client>
{
    Task<Result<Client?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Client>>> GetClientsByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    );
}
