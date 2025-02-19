using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories;

public class ClientRepository(ApplicationDbContext context)
    : BaseRepository<Client>(context),
        IClientRepository
{
    public async Task<Result<Client?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var client = await _entities
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return Result<Client?>.Success(client);
        }
        catch (Exception ex)
        {
            return Result<Client?>.Failure($"Failed to get client with ID {id}: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var exists = await _entities.AnyAsync(c => c.Id == id, cancellationToken);
            return Result<bool>.Success(exists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to check if client exists: {ex.Message}");
        }
    }

    public async Task<Result> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = await _entities.FindAsync([id], cancellationToken);
            if (client == null)
                return Result.Failure($"Client with ID {id} not found.");

            return await DeleteAsync(client, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete client: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Client>>> GetClientsByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var clients = await _entities
                .Where(c => c.Name.Contains(name))
                .Include(c => c.Projects)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Client>>.Success(clients);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Client>>.Failure(
                $"Failed to get clients by name '{name}': {ex.Message}"
            );
        }
    }

    public override async Task<Result<IEnumerable<Client>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var clients = await _entities.Include(c => c.Projects).ToListAsync(cancellationToken);
            return Result<IEnumerable<Client>>.Success(clients);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Client>>.Failure($"Failed to get clients: {ex.Message}");
        }
    }
}
