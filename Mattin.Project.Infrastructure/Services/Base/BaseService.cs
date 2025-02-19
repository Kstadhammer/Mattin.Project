// Base service implementation enhanced with AI assistance for:
// - Generic CRUD operations
// - Error handling patterns
// - Repository integration
// - Common business logic

using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Infrastructure.Services.Base;

/// <summary>
/// Base service providing common CRUD operations and error handling for all services.
/// </summary>
public abstract class BaseService<TEntity>(IBaseRepository<TEntity> repository)
    : IBaseService<TEntity>
    where TEntity : BaseEntity
{
    protected readonly IBaseRepository<TEntity> _repository = repository;

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(
                $"Failed to retrieve {typeof(TEntity).Name} entities: {result.Error}"
            );

        return result.Value;
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(
                $"Failed to retrieve {typeof(TEntity).Name} with ID {id}: {result.Error}"
            );

        return result.Value.FirstOrDefault(e => e.Id == id);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var result = await _repository.AddAsync(entity);
        if (result.IsFailure)
            throw new InvalidOperationException(
                $"Failed to create {typeof(TEntity).Name}: {result.Error}"
            );

        return result.Value;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var existsResult = await _repository.GetAllAsync();
        if (existsResult.IsFailure)
            throw new InvalidOperationException(
                $"Failed to verify existence of {typeof(TEntity).Name}: {existsResult.Error}"
            );

        if (existsResult.Value.All(e => e.Id != entity.Id))
            throw new KeyNotFoundException(
                $"{typeof(TEntity).Name} with ID {entity.Id} not found."
            );

        var result = await _repository.UpdateAsync(entity);
        if (result.IsFailure)
            throw new InvalidOperationException(
                $"Failed to update {typeof(TEntity).Name}: {result.Error}"
            );

        return result.Value;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException($"Failed to retrieve entity: {result.Error}");

        var entity = result.Value.FirstOrDefault(e => e.Id == id);
        if (entity == null)
            return false;

        var deleteResult = await _repository.DeleteAsync(entity);
        return deleteResult.IsSuccess;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(
                $"Failed to check existence of {typeof(TEntity).Name} with ID {id}: {result.Error}"
            );

        return result.Value.Any(e => e.Id == id);
    }
}
