using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories;

public class ClientRepository(ApplicationDbContext context) : BaseRepository<Client>(context), IClientRepository
{
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
