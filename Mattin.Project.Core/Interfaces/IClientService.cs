using Mattin.Project.Core.Common;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.DTOs.Project;

namespace Mattin.Project.Core.Interfaces;

public interface IClientService
{
    // Get all clients
    Task<Result<IEnumerable<ClientDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    );

    // Get client by id
    Task<Result<ClientDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );

    // Create client
    Task<Result<ClientDetailsDto>> CreateAsync(
        CreateClientDto dto,
        CancellationToken cancellationToken = default
    );

    // Update client
    Task<Result<ClientDetailsDto>> UpdateAsync(
        UpdateClientDto dto,
        CancellationToken cancellationToken = default
    );

    // Delete client
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);

    // Check if client exists
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);

    // Get clients by name
    Task<Result<IEnumerable<ClientDetailsDto>>> GetClientsByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    // Get client projects
    Task<Result<IEnumerable<ProjectDetailsDto>>> GetClientProjectsAsync(
        int clientId,
        CancellationToken cancellationToken = default
    );
}
