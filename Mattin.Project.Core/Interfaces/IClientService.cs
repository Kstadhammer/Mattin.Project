using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.DTOs.Project;

namespace Mattin.Project.Core.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientDetailsDto>> GetAllAsync();
    Task<ClientDetailsDto?> GetByIdAsync(int id);
    Task<ClientDetailsDto> CreateAsync(CreateClientDto dto);
    Task<ClientDetailsDto> UpdateAsync(UpdateClientDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);

    Task<IEnumerable<ClientDetailsDto>> GetClientsByNameAsync(string name);
    Task<IEnumerable<ProjectDetailsDto>> GetClientProjectsAsync(int clientId);
}
