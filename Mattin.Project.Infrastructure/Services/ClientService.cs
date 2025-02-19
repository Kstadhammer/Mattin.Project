using AutoMapper;
using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Infrastructure.Services;

public class ClientService(
    IClientRepository clientRepository,
    IProjectRepository projectRepository,
    IMapper mapper)
    : IClientService
{
    public async Task<IEnumerable<ClientDetailsDto>> GetAllAsync()
    {
        var result = await clientRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return mapper.Map<IEnumerable<ClientDetailsDto>>(result.Value);
    }

    public async Task<ClientDetailsDto?> GetByIdAsync(int id)
    {
        var result = await clientRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        var client = result.Value.FirstOrDefault(c => c.Id == id);
        return mapper.Map<ClientDetailsDto>(client);
    }

    public async Task<ClientDetailsDto> CreateAsync(CreateClientDto dto)
    {
        var entity = mapper.Map<Client>(dto);
        var result = await clientRepository.AddAsync(entity);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return mapper.Map<ClientDetailsDto>(result.Value);
    }

    public async Task<ClientDetailsDto> UpdateAsync(UpdateClientDto dto)
    {
        var result = await clientRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        if (result.Value.All(c => c.Id != dto.Id))
            throw new KeyNotFoundException($"Client with ID {dto.Id} not found.");

        var entity = mapper.Map<Client>(dto);
        var updateResult = await clientRepository.UpdateAsync(entity);
        if (updateResult.IsFailure)
            throw new InvalidOperationException(updateResult.Error);

        return mapper.Map<ClientDetailsDto>(updateResult.Value);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await clientRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        var client = result.Value.FirstOrDefault(c => c.Id == id);
        if (client == null)
            return false;

        if (client.Projects.Any())
            throw new InvalidOperationException("Cannot delete a client with active projects.");

        var deleteResult = await clientRepository.DeleteAsync(id);
        return deleteResult.IsSuccess;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await clientRepository.GetAllAsync();
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return result.Value.Any(c => c.Id == id);
    }

    public async Task<IEnumerable<ClientDetailsDto>> GetClientsByNameAsync(string name)
    {
        var result = await clientRepository.GetClientsByNameAsync(name);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error);

        return mapper.Map<IEnumerable<ClientDetailsDto>>(result.Value);
    }

    public async Task<IEnumerable<ProjectDetailsDto>> GetClientProjectsAsync(int clientId)
    {
        var result = await projectRepository.GetProjectsByClientIdAsync(clientId);
        return mapper.Map<IEnumerable<ProjectDetailsDto>>(result);
    }
}
