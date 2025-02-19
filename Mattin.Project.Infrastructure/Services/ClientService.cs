using AutoMapper;
using Mattin.Project.Core.Common;
using Mattin.Project.Core.Factories;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Infrastructure.Services;

/// <summary>
/// Manages client-related operations including project associations.
/// Handles client data validation and relationship management.
/// </summary>
public class ClientService(
    IClientRepository clientRepository,
    IProjectRepository projectRepository,
    IMappingFactory mappingFactory
) : IClientService
{
    public async Task<Result<IEnumerable<ClientDetailsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await clientRepository.GetAllAsync(cancellationToken);
        if (result.IsFailure)
            return Result<IEnumerable<ClientDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ClientDetailsDto>>.Success(
            mappingFactory.CreateClientDetailsDtos(result.Value)
        );
    }

    public async Task<Result<ClientDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await clientRepository.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return Result<ClientDetailsDto?>.Failure(result.Error);

        return Result<ClientDetailsDto?>.Success(
            result.Value == null ? null : mappingFactory.CreateClientDetailsDto(result.Value)
        );
    }

    public async Task<Result<ClientDetailsDto>> CreateAsync(
        CreateClientDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var entity = mappingFactory.CreateClientEntity(dto);
        entity.Created = DateTime.UtcNow;

        var result = await clientRepository.AddAsync(entity, cancellationToken);
        if (result.IsFailure)
            return Result<ClientDetailsDto>.Failure(result.Error);

        return Result<ClientDetailsDto>.Success(
            mappingFactory.CreateClientDetailsDto(result.Value)
        );
    }

    public async Task<Result<ClientDetailsDto>> UpdateAsync(
        UpdateClientDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var existingResult = await clientRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (existingResult.IsFailure)
            return Result<ClientDetailsDto>.Failure(existingResult.Error);

        if (existingResult.Value == null)
            return Result<ClientDetailsDto>.Failure($"Client with ID {dto.Id} not found.");

        var client = mappingFactory.UpdateClientEntity(dto, existingResult.Value);
        client.Modified = DateTime.UtcNow;

        var updateResult = await clientRepository.UpdateAsync(client, cancellationToken);
        if (updateResult.IsFailure)
            return Result<ClientDetailsDto>.Failure(updateResult.Error);

        return Result<ClientDetailsDto>.Success(
            mappingFactory.CreateClientDetailsDto(updateResult.Value)
        );
    }

    public async Task<Result<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var clientResult = await GetByIdAsync(id, cancellationToken);
        if (clientResult.IsFailure)
            return Result<bool>.Failure(clientResult.Error);

        if (clientResult.Value == null)
            return Result<bool>.Failure($"Client with ID {id} not found for deletion.");

        var projectsResult = await projectRepository.GetProjectsByClientIdAsync(
            id,
            cancellationToken
        );
        if (projectsResult.IsFailure)
            return Result<bool>.Failure(projectsResult.Error);

        if (projectsResult.Value.Any())
            return Result<bool>.Failure(
                $"Cannot delete client with ID {id}: Client has {projectsResult.Value.Count()} active projects. Please delete or reassign projects first."
            );

        var result = await clientRepository.DeleteByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Result<bool>.Success(true) : Result<bool>.Failure(result.Error);
    }

    public async Task<Result<bool>> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await clientRepository.ExistsAsync(id, cancellationToken);
        return result.IsSuccess
            ? Result<bool>.Success(result.Value)
            : Result<bool>.Failure(result.Error);
    }

    public async Task<Result<IEnumerable<ClientDetailsDto>>> GetClientsByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        var result = await clientRepository.GetClientsByNameAsync(name, cancellationToken);
        if (result.IsFailure)
            return Result<IEnumerable<ClientDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ClientDetailsDto>>.Success(
            mappingFactory.CreateClientDetailsDtos(result.Value)
        );
    }

    public async Task<Result<IEnumerable<ProjectDetailsDto>>> GetClientProjectsAsync(
        int clientId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await projectRepository.GetProjectsByClientIdAsync(
            clientId,
            cancellationToken
        );
        if (result.IsFailure)
            return Result<IEnumerable<ProjectDetailsDto>>.Failure(result.Error);

        return Result<IEnumerable<ProjectDetailsDto>>.Success(
            mappingFactory.CreateProjectDetailsDtos(result.Value)
        );
    }
}
