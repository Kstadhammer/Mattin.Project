using AutoMapper;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Mappings;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        // ProjectEntity -> ProjectDetailsDto
        CreateMap<ProjectEntity, ProjectDetailsDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

        // CreateProjectDto -> ProjectEntity
        CreateMap<CreateProjectDto, ProjectEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.StatusId, opt => opt.Ignore())
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectManager, opt => opt.Ignore());

        // UpdateProjectDto -> ProjectEntity
        CreateMap<UpdateProjectDto, ProjectEntity>()
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectManager, opt => opt.Ignore());

        // ProjectEntity -> UpdateProjectDto
        CreateMap<ProjectEntity, UpdateProjectDto>();
    }
}
