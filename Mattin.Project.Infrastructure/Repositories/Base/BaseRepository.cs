using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories.Base;

public abstract class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var entities = await _entities.ToListAsync(cancellationToken);
            return Result<IEnumerable<TEntity>>.Success(entities);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TEntity>>.Failure($"Failed to get entities: {ex.Message}");
        }
    }

    public virtual async Task<Result<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await _entities.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            return Result<TEntity>.Failure($"Failed to add entity: {ex.Message}");
        }
    }

    public virtual async Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            return Result<TEntity>.Failure($"Failed to update entity: {ex.Message}");
        }
    }

    public virtual async Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var entity = await _entities.FindAsync([id], cancellationToken);
            if (entity == null)
                return Result.Failure($"Entity with ID {id} not found.");

            _entities.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete entity: {ex.Message}");
        }
    }
}
