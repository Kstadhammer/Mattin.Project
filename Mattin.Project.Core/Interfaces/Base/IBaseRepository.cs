using Mattin.Project.Core.Common;

namespace Mattin.Project.Core.Interfaces.Base;

public interface IBaseRepository<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : class
{
    Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    );
    Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
