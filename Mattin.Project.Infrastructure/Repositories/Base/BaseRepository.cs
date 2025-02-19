// Base repository implementation enhanced with AI assistance for:
// - Generic database operations
// - Entity Framework Core integration
// - Error handling and logging
// - Transaction management
// - Performance optimization
// - Refactoring for better readability and maintainability

using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories.Base;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    private bool _disposed;
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _entities;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    protected Result CheckDisposed()
    {
        return _disposed ? Result.Failure("Repository has been disposed.") : Result.Success();
    }

    public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<IEnumerable<TEntity>>.Failure(disposedCheck.Error);

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
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<TEntity>.Failure(disposedCheck.Error);

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
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<TEntity>.Failure(disposedCheck.Error);

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
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result.Failure(disposedCheck.Error);

        try
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete entity: {ex.Message}");
        }
    }

    protected virtual async Task DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                await _context.DisposeAsync();
            }
            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~BaseRepository()
    {
        Dispose(false);
    }
}
