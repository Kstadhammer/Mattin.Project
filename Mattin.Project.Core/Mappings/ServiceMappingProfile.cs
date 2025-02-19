// Service mapping profile enhanced with AI assistance for:
// - DTO transformations
// - Property mapping rules
// - Relationship handling
// - Data validation

using AutoMapper;
using Mattin.Project.Core.Models.DTOs.Service;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        // Service -> ServiceDetailsDto
        CreateMap<Service, ServiceDetailsDto>();

        // CreateServiceDto -> Service
        CreateMap<CreateServiceDto, Service>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        // UpdateServiceDto -> Service
        CreateMap<UpdateServiceDto, Service>()
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore());

        // Service -> UpdateServiceDto
        CreateMap<Service, UpdateServiceDto>();
    }
}
