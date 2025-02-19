using Mattin.Project.Core.Common;
using Mattin.Project.Core.Models.DTOs.Service;

namespace Mattin.Project.Core.Interfaces;

public interface IServiceService
{
    Task<Result<IEnumerable<ServiceDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    );
    Task<Result<ServiceDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<Result<ServiceDetailsDto>> CreateAsync(
        CreateServiceDto dto,
        CancellationToken cancellationToken = default
    );
    Task<Result<ServiceDetailsDto>> UpdateAsync(
        UpdateServiceDto dto,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ServiceDetailsDto>>> GetServicesByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<ServiceDetailsDto>>> GetActiveServicesAsync(
        CancellationToken cancellationToken = default
    );
}
