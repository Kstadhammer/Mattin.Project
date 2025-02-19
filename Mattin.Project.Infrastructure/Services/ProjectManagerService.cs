using AutoMapper;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.ProjectManager;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Services.Base;

namespace Mattin.Project.Infrastructure.Services;

public class ProjectManagerService(
    IProjectManagerRepository projectManagerRepository,
    IMapper mapper
) : BaseService<ProjectManager>(projectManagerRepository), IProjectManagerService
{
    public async Task<IEnumerable<ProjectManagerDetailsDto>> GetAllAsync()
    {
        var result = await projectManagerRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return mapper.Map<IEnumerable<ProjectManagerDetailsDto>>(result.Value);
    }

    public async Task<ProjectManagerDetailsDto?> GetByIdAsync(int id)
    {
        var result = await projectManagerRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        var projectManager = result.Value.FirstOrDefault(p => p.Id == id);
        return mapper.Map<ProjectManagerDetailsDto>(projectManager);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await projectManagerRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value.Any(p => p.Id == id);
    }
}
