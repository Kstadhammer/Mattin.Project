using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories;

public class ProjectManagerRepository(ApplicationDbContext context)
    : BaseRepository<ProjectManager>(context),
        IProjectManagerRepository
{
    public override async Task<Result<IEnumerable<ProjectManager>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var projectManagers = await _entities
                .Include(pm => pm.Projects)
                .ToListAsync(cancellationToken);
            return Result<IEnumerable<ProjectManager>>.Success(projectManagers);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProjectManager>>.Failure(
                $"Failed to get project managers: {ex.Message}"
            );
        }
    }
}
