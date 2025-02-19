// Service factory implementation enhanced with AI assistance for:
// - Dependency injection
// - Service instantiation
// - Factory pattern implementation
// - Cross-cutting concerns

using AutoMapper;
using Mattin.Project.Core.Factories;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories;
using Mattin.Project.Infrastructure.Services;

namespace Mattin.Project.Infrastructure.Factories;

public class ServiceFactory(
    IRepositoryFactory repositoryFactory,
    IMappingFactory mappingFactory,
    ApplicationDbContext context
) : IServiceFactory
{
    public IProjectService CreateProjectService()
    {
        var projectRepository = repositoryFactory.CreateProjectRepository();
        var statusRepository = repositoryFactory.CreateStatusRepository();
        return new ProjectService(projectRepository, statusRepository, mappingFactory, context);
    }

    public IClientService CreateClientService()
    {
        var clientRepository = repositoryFactory.CreateClientRepository();
        var projectRepository = repositoryFactory.CreateProjectRepository();
        return new ClientService(clientRepository, projectRepository, mappingFactory);
    }

    public IProjectManagerService CreateProjectManagerService()
    {
        var projectManagerRepository = repositoryFactory.CreateProjectManagerRepository();
        return new ProjectManagerService(projectManagerRepository, mappingFactory);
    }

    public IServiceService CreateServiceService()
    {
        var serviceRepository = repositoryFactory.CreateServiceRepository();
        return new ServiceService(serviceRepository, mappingFactory);
    }
}
