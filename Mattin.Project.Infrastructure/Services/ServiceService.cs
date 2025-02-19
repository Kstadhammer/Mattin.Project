// Service implementation enhanced with AI assistance for:
// - Service management operations
// - DTO mapping and transformations
// - Error handling and validation
// - Category-based filtering
// - Active service management

using Mattin.Project.Core.Common;
using Mattin.Project.Core.Factories;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Service;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Infrastructure.Services.Base;

namespace Mattin.Project.Infrastructure.Services;

public class ServiceService(IServiceRepository serviceRepository, IMappingFactory mappingFactory)
    : BaseService<Service>(serviceRepository),
        IServiceService
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    public async Task<Result<IEnumerable<ServiceDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _serviceRepository.GetAllAsync(cancellationToken);
        if (result.IsFailure)
            return Result<IEnumerable<ServiceDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ServiceDetailsDto>>.Success(
            mappingFactory.CreateServiceDetailsDtos(result.Value)
        );
    }

    public async Task<Result<ServiceDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _serviceRepository.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return Result<ServiceDetailsDto?>.Failure(result.Error);

        return Result<ServiceDetailsDto?>.Success(
            result.Value == null ? null : mappingFactory.CreateServiceDetailsDto(result.Value)
        );
    }

    public async Task<Result<ServiceDetailsDto>> CreateAsync(
        CreateServiceDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var entity = mappingFactory.CreateServiceEntity(dto);
        entity.Created = DateTime.UtcNow;

        var result = await _serviceRepository.AddAsync(entity, cancellationToken);
        if (result.IsFailure)
            return Result<ServiceDetailsDto>.Failure(result.Error);

        return Result<ServiceDetailsDto>.Success(
            mappingFactory.CreateServiceDetailsDto(result.Value)
        );
    }

    public async Task<Result<ServiceDetailsDto>> UpdateAsync(
        UpdateServiceDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var existingResult = await _serviceRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (existingResult.IsFailure)
            return Result<ServiceDetailsDto>.Failure(existingResult.Error);

        if (existingResult.Value == null)
            return Result<ServiceDetailsDto>.Failure($"Service with ID {dto.Id} not found.");

        var service = mappingFactory.UpdateServiceEntity(dto, existingResult.Value);
        service.Modified = DateTime.UtcNow;

        var updateResult = await _serviceRepository.UpdateAsync(service, cancellationToken);
        if (updateResult.IsFailure)
            return Result<ServiceDetailsDto>.Failure(updateResult.Error);

        return Result<ServiceDetailsDto>.Success(
            mappingFactory.CreateServiceDetailsDto(updateResult.Value)
        );
    }

    public async Task<Result<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var serviceResult = await GetByIdAsync(id, cancellationToken);
        if (serviceResult.IsFailure)
            return Result<bool>.Failure(serviceResult.Error);

        if (serviceResult.Value == null)
            return Result<bool>.Failure($"Service with ID {id} not found for deletion.");

        var result = await _serviceRepository.DeleteByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Result<bool>.Success(true) : Result<bool>.Failure(result.Error);
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _serviceRepository.ExistsAsync(id, cancellationToken);
        return result.IsSuccess
            ? Result<bool>.Success(result.Value)
            : Result<bool>.Failure(result.Error);
    }

    public async Task<Result<IEnumerable<ServiceDetailsDto>>> GetServicesByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _serviceRepository.GetServicesByCategoryAsync(
            category,
            cancellationToken
        );
        if (result.IsFailure)
            return Result<IEnumerable<ServiceDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ServiceDetailsDto>>.Success(
            mappingFactory.CreateServiceDetailsDtos(result.Value)
        );
    }

    public async Task<Result<IEnumerable<ServiceDetailsDto>>> GetActiveServicesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _serviceRepository.GetActiveServicesAsync(cancellationToken);
        if (result.IsFailure)
            return Result<IEnumerable<ServiceDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ServiceDetailsDto>>.Success(
            mappingFactory.CreateServiceDetailsDtos(result.Value)
        );
    }
}
