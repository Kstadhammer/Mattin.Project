// Service factory implementation enhanced with AI assistance for:
// - Dependency injection
// - Service instantiation
// - Factory pattern implementation
// - Cross-cutting concerns

using AutoMapper;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Infrastructure.Services;

namespace Mattin.Project.Infrastructure.Factories;

public class ServiceFactory : IServiceFactory
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IMapper _mapper;

    public ServiceFactory(IRepositoryFactory repositoryFactory, IMapper mapper)
    {
        _repositoryFactory = repositoryFactory;
        _mapper = mapper;
    }

    public IProjectService CreateProjectService()
    {
        var projectRepository = _repositoryFactory.CreateProjectRepository();
        var statusRepository = _repositoryFactory.CreateStatusRepository();
        return new ProjectService(projectRepository, statusRepository, _mapper);
    }

    public IClientService CreateClientService()
    {
        var clientRepository = _repositoryFactory.CreateClientRepository();
        var projectRepository = _repositoryFactory.CreateProjectRepository();
        return new ClientService(clientRepository, projectRepository, _mapper);
    }

    public IProjectManagerService CreateProjectManagerService()
    {
        var projectManagerRepository = _repositoryFactory.CreateProjectManagerRepository();
        return new ProjectManagerService(projectManagerRepository, _mapper);
    }
}
