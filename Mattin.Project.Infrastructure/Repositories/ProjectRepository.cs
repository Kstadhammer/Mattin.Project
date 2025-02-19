using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Mattin.Project.Infrastructure.Repositories;

public class ProjectRepository(ApplicationDbContext context)
    : BaseRepository<ProjectEntity>(context),
        IProjectRepository
{
    public async Task<Result<string>> GenerateProjectNumberAsync()
    {
        try
        {
            var year = DateTime.UtcNow.Year;
            var lastProject = await _entities
                .Where(p => p.ProjectNumber.StartsWith($"{year}-"))
                .OrderByDescending(p => p.ProjectNumber)
                .FirstOrDefaultAsync();

            var projectNumber =
                lastProject == null
                    ? $"{year}-001"
                    : $"{year}-{(int.Parse(lastProject.ProjectNumber.Split('-')[1]) + 1):000}";

            return Result<string>.Success(projectNumber);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Failed to generate project number: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ProjectEntity>>> GetProjectsByClientIdAsync(int clientId)
    {
        try
        {
            var projects = await _entities
                .Where(p => p.ClientId == clientId)
                .Include(p => p.Client)
                .Include(p => p.Status)
                .ToListAsync();

            return Result<IEnumerable<ProjectEntity>>.Success(projects);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProjectEntity>>.Failure(
                $"Failed to get projects for client {clientId}: {ex.Message}"
            );
        }
    }

    public override async Task<Result<IEnumerable<ProjectEntity>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Console.WriteLine("Debug: Fetching all projects from database");
            var projects = await _entities
                .Include(p => p.Client)
                .Include(p => p.Status)
                .Include(p => p.ProjectManager)
                .ToListAsync(cancellationToken);
            Console.WriteLine($"Debug: Found {projects.Count} projects");
            return Result<IEnumerable<ProjectEntity>>.Success(projects);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProjectEntity>>.Failure(
                $"Failed to get projects: {ex.Message}"
            );
        }
    }
}
