using Mattin.Project.Core.Interfaces.Base;

namespace Mattin.Project.Core.Interfaces.Factories;

public interface IRepositoryFactory
{
    IProjectRepository CreateProjectRepository();
    IClientRepository CreateClientRepository();
    IStatusRepository CreateStatusRepository();
    IProjectManagerRepository CreateProjectManagerRepository();
}
