using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories;

public class StatusRepository(ApplicationDbContext context) : BaseRepository<Status>(context), IStatusRepository
{
    private readonly ApplicationDbContext _dbContext = context;

    public async Task<Result<Status?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var status = await _dbContext
                .Set<Status>()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            return Result<Status?>.Success(status);
        }
        catch (Exception ex)
        {
            return Result<Status?>.Failure($"Failed to get status: {ex.Message}");
        }
    }

    public async Task<Result<Status?>> GetDefaultStatusAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var status = await _dbContext
                .Set<Status>()
                .FirstOrDefaultAsync(s => s.IsDefault, cancellationToken);

            return Result<Status?>.Success(status);
        }
        catch (Exception ex)
        {
            return Result<Status?>.Failure($"Failed to get default status: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var exists = await _dbContext
                .Set<Status>()
                .AnyAsync(s => s.Id == id, cancellationToken);

            return Result<bool>.Success(exists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to check status existence: {ex.Message}");
        }
    }
}
