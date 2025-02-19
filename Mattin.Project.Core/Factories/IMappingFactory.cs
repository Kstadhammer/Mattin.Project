using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Factories;

public interface IMappingFactory
{
    // Project mappings
    ProjectEntity CreateProjectEntity(CreateProjectDto dto);
    ProjectEntity UpdateProjectEntity(UpdateProjectDto dto, ProjectEntity existing);
    ProjectDetailsDto CreateProjectDetailsDto(ProjectEntity entity);
    IEnumerable<ProjectDetailsDto> CreateProjectDetailsDtos(IEnumerable<ProjectEntity> entities);

    // Client mappings
    Client CreateClientEntity(CreateClientDto dto);
    Client UpdateClientEntity(UpdateClientDto dto, Client existing);
    ClientDetailsDto CreateClientDetailsDto(Client entity);
    IEnumerable<ClientDetailsDto> CreateClientDetailsDtos(IEnumerable<Client> entities);
}
