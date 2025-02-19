// Client mapping profile enhanced with AI assistance for:
// - DTO transformations
// - Property mapping rules
// - Relationship handling
// - Data validation

using AutoMapper;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Mappings;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        // Client -> ClientDetailsDto
        CreateMap<Client, ClientDetailsDto>();

        // CreateClientDto -> Client
        CreateMap<CreateClientDto, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore());

        // UpdateClientDto -> Client
        CreateMap<UpdateClientDto, Client>()
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Modified, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore());

        // Client -> UpdateClientDto
        CreateMap<Client, UpdateClientDto>();
    }
}
