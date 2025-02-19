using Mattin.Project.Core.Models.DTOs.ProjectManager;

namespace Mattin.Project.Core.Interfaces;

public interface IProjectManagerService
{
    Task<IEnumerable<ProjectManagerDetailsDto>> GetAllAsync();
    Task<ProjectManagerDetailsDto?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
}
