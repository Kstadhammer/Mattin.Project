// Repository implementation enhanced with AI assistance for:
// - Project data access and persistence
// - Project number generation
// - Relationship management with clients and status
// - Transaction support
// - Error handling and validation

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
    public async Task<Result<ProjectEntity?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<ProjectEntity?>.Failure(disposedCheck.Error);

        try
        {
            var project = await _entities
                .Include(p => p.Client)
                .Include(p => p.Status)
                .Include(p => p.ProjectManager)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return Result<ProjectEntity?>.Success(project);
        }
        catch (Exception ex)
        {
            return Result<ProjectEntity?>.Failure(
                $"Failed to get project with ID {id}: {ex.Message}"
            );
        }
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<bool>.Failure(disposedCheck.Error);

        try
        {
            var exists = await _entities.AnyAsync(p => p.Id == id, cancellationToken);
            return Result<bool>.Success(exists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to check if project exists: {ex.Message}");
        }
    }

    public async Task<Result> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result.Failure(disposedCheck.Error);

        try
        {
            var project = await _entities.FindAsync([id], cancellationToken);
            if (project == null)
                return Result.Failure($"Project with ID {id} not found.");

            return await DeleteAsync(project, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete project: {ex.Message}");
        }
    }

    public async Task<Result<string>> GenerateProjectNumberAsync(
        CancellationToken cancellationToken = default
    )
    {
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<string>.Failure(disposedCheck.Error);

        try
        {
            var year = DateTime.UtcNow.Year;
            var lastProject = await _entities
                .Where(p => p.ProjectNumber.StartsWith($"{year}-"))
                .OrderByDescending(p => p.ProjectNumber)
                .FirstOrDefaultAsync(cancellationToken);

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

    public async Task<Result<IEnumerable<ProjectEntity>>> GetProjectsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken = default
    )
    {
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<IEnumerable<ProjectEntity>>.Failure(disposedCheck.Error);

        try
        {
            var projects = await _entities
                .Where(p => p.ClientId == clientId)
                .Include(p => p.Client)
                .Include(p => p.Status)
                .Include(p => p.Service)
                .ToListAsync(cancellationToken);

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
        var disposedCheck = CheckDisposed();
        if (disposedCheck.IsFailure)
            return Result<IEnumerable<ProjectEntity>>.Failure(disposedCheck.Error);

        try
        {
            var projects = await _entities
                .Include(p => p.Client)
                .Include(p => p.Status)
                .Include(p => p.ProjectManager)
                .Include(p => p.Service)
                .ToListAsync(cancellationToken);
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
