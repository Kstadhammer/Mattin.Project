using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDetailsDto>> GetAllAsync();
    Task<ProjectDetailsDto?> GetByIdAsync(int id);
    Task<ProjectDetailsDto> CreateAsync(CreateProjectDto dto);
    Task<ProjectDetailsDto> UpdateAsync(UpdateProjectDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);

    Task<string> GenerateProjectNumberAsync();
    Task<IEnumerable<ProjectDetailsDto>> GetProjectsByClientIdAsync(int clientId);
    Task<bool> UpdateProjectStatusAsync(int projectId, string status);
}
