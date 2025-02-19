using AutoMapper;
using Mattin.Project.Core.Models.DTOs.ProjectManager;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Mappings;

public class ProjectManagerMappingProfile : Profile
{
    public ProjectManagerMappingProfile()
    {
        CreateMap<ProjectManager, ProjectManagerDetailsDto>();
    }
}
