// Repository implementation enhanced with AI assistance for:
// - Service data access and persistence
// - Category-based filtering
// - Active service filtering
// - Error handling and validation
// - Eager loading of related entities

using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories;

public class ServiceRepository(ApplicationDbContext context)
    : BaseRepository<Service>(context),
        IServiceRepository
{
    public async Task<Result<Service?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var service = await _entities
                .Include(s => s.Projects)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            return Result<Service?>.Success(service);
        }
        catch (Exception ex)
        {
            return Result<Service?>.Failure($"Failed to get service with ID {id}: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var exists = await _entities.AnyAsync(s => s.Id == id, cancellationToken);
            return Result<bool>.Success(exists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to check if service exists: {ex.Message}");
        }
    }

    public async Task<Result> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var service = await _entities.FindAsync([id], cancellationToken);
            if (service == null)
                return Result.Failure($"Service with ID {id} not found.");

            return await DeleteAsync(service, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete service: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Service>>> GetServicesByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var services = await _entities
                .Where(s => s.Category == category)
                .Include(s => s.Projects)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Service>>.Success(services);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Service>>.Failure(
                $"Failed to get services by category '{category}': {ex.Message}"
            );
        }
    }

    public async Task<Result<IEnumerable<Service>>> GetActiveServicesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var services = await _entities
                .Where(s => s.IsActive)
                .Include(s => s.Projects)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Service>>.Success(services);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Service>>.Failure(
                $"Failed to get active services: {ex.Message}"
            );
        }
    }

    public override async Task<Result<IEnumerable<Service>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            Console.WriteLine($"Debug - Database Path: {connection.ConnectionString}");
            Console.WriteLine($"Debug - Database State: {_context.Database.CanConnect()}");

            var services = await _entities
                .Include(s => s.Projects)
                .AsNoTracking() // Add this to improve performance and reduce memory usage
                .ToListAsync(cancellationToken);

            Console.WriteLine($"Debug - Services Count: {services.Count}");
            return Result<IEnumerable<Service>>.Success(services);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Debug - Exception Details: {ex}");
            return Result<IEnumerable<Service>>.Failure($"Failed to get services: {ex.Message}");
        }
    }
}
