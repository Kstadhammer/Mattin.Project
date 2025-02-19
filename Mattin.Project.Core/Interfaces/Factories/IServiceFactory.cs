using Mattin.Project.Core.Interfaces.Base;

namespace Mattin.Project.Core.Interfaces.Factories;

public interface IServiceFactory
{
    IProjectService CreateProjectService();
    IClientService CreateClientService();
    IProjectManagerService CreateProjectManagerService();
}
