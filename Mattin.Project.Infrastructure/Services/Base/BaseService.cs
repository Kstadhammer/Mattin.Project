using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Infrastructure.Services.Base;

public abstract class BaseService<TEntity>(IBaseRepository<TEntity> repository) : IBaseService<TEntity>
    where TEntity : BaseEntity
{
    protected readonly IBaseRepository<TEntity> _repository = repository;

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value;
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value.FirstOrDefault(e => e.Id == id);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var result = await _repository.AddAsync(entity);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var existsResult = await _repository.GetAllAsync();
        if (existsResult.IsFailure)
            throw new InvalidOperationException(existsResult.Error);

        if (existsResult.Value.All(e => e.Id != entity.Id))
            throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");

        var result = await _repository.UpdateAsync(entity);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        return result.IsSuccess;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value.Any(e => e.Id == id);
    }
}
