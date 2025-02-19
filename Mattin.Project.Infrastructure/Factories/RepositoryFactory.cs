using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Repositories;

namespace Mattin.Project.Infrastructure.Factories;

public class RepositoryFactory(ApplicationDbContext context) : IRepositoryFactory
{
    public IProjectRepository CreateProjectRepository()
    {
        return new ProjectRepository(context);
    }

    public IClientRepository CreateClientRepository()
    {
        return new ClientRepository(context);
    }

    public IStatusRepository CreateStatusRepository()
    {
        return new StatusRepository(context);
    }

    public IProjectManagerRepository CreateProjectManagerRepository()
    {
        return new ProjectManagerRepository(context);
    }
}
