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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Get the current assembly location
        var assemblyLocation = typeof(DependencyInjection).Assembly.Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;

        // Navigate up to solution root (3 levels up from bin/Debug/net9.0)
        var solutionDir = Path.GetFullPath(Path.Combine(assemblyDirectory, "../../.."));
        var dataDir = Path.Combine(solutionDir, "Data");

        // Create the directory if it doesn't exist
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir);
        }

        var dbPath = Path.Combine(dataDir, "mattinproject.db");
        Console.WriteLine($"Database path: {dbPath}"); // Debug line

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
