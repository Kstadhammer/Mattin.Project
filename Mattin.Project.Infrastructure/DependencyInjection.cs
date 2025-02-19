// Dependency injection configuration developed with AI assistance
// Implements clean architecture patterns and service registration

using AutoMapper;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Core.Mappings;
using Mattin.Project.Infrastructure.Contexts;
using Mattin.Project.Infrastructure.Factories;
using Mattin.Project.Infrastructure.Repositories;
using Mattin.Project.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mattin.Project.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string dbPath
    )
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}")
        );

        // AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ProjectMappingProfile>();
            cfg.AddProfile<ClientMappingProfile>();
            cfg.AddProfile<ProjectManagerMappingProfile>();
        });

        // Repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IProjectManagerRepository, ProjectManagerRepository>();

        // Services
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProjectManagerService, ProjectManagerService>();

        // Factories
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IServiceFactory, ServiceFactory>();

        return services;
    }
}
