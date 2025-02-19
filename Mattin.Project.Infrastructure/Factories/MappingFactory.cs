// Mapping factory implementation enhanced with AI assistance for:
// - Entity to DTO transformations
// - DTO to entity transformations
// - AutoMapper integration
// - Default value handling
// - Collection mapping support
// - Clean architecture pattern implementation

using AutoMapper;
using Mattin.Project.Core.Factories;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.DTOs.ProjectManager;
using Mattin.Project.Core.Models.DTOs.Service;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Infrastructure.Factories;

public class MappingFactory(IMapper mapper) : IMappingFactory
{
    public ProjectEntity CreateProjectEntity(CreateProjectDto dto)
    {
        var entity = mapper.Map<ProjectEntity>(dto);
        entity.StatusId = 1; // Default status
        return entity;
    }

    public ProjectEntity UpdateProjectEntity(UpdateProjectDto dto, ProjectEntity existing)
    {
        mapper.Map(dto, existing);
        return existing;
    }

    public ProjectDetailsDto CreateProjectDetailsDto(ProjectEntity entity)
    {
        return mapper.Map<ProjectDetailsDto>(entity);
    }

    public IEnumerable<ProjectDetailsDto> CreateProjectDetailsDtos(
        IEnumerable<ProjectEntity> entities
    )
    {
        return mapper.Map<IEnumerable<ProjectDetailsDto>>(entities);
    }

    public Client CreateClientEntity(CreateClientDto dto)
    {
        return mapper.Map<Client>(dto);
    }

    public Client UpdateClientEntity(UpdateClientDto dto, Client existing)
    {
        mapper.Map(dto, existing);
        return existing;
    }

    public ClientDetailsDto CreateClientDetailsDto(Client entity)
    {
        return mapper.Map<ClientDetailsDto>(entity);
    }

    public IEnumerable<ClientDetailsDto> CreateClientDetailsDtos(IEnumerable<Client> entities)
    {
        return mapper.Map<IEnumerable<ClientDetailsDto>>(entities);
    }

    public ProjectManagerDetailsDto CreateProjectManagerDetailsDto(ProjectManager entity)
    {
        return mapper.Map<ProjectManagerDetailsDto>(entity);
    }

    public IEnumerable<ProjectManagerDetailsDto> CreateProjectManagerDetailsDtos(
        IEnumerable<ProjectManager> entities
    )
    {
        return mapper.Map<IEnumerable<ProjectManagerDetailsDto>>(entities);
    }

    // Service mappings
    public Service CreateServiceEntity(CreateServiceDto dto)
    {
        return mapper.Map<Service>(dto);
    }

    public Service UpdateServiceEntity(UpdateServiceDto dto, Service existing)
    {
        mapper.Map(dto, existing);
        return existing;
    }

    public ServiceDetailsDto CreateServiceDetailsDto(Service entity)
    {
        return mapper.Map<ServiceDetailsDto>(entity);
    }

    public IEnumerable<ServiceDetailsDto> CreateServiceDetailsDtos(IEnumerable<Service> entities)
    {
        return mapper.Map<IEnumerable<ServiceDetailsDto>>(entities);
    }
}
