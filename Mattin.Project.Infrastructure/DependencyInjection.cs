// Dependency injection configuration developed with AI assistance
// Implements clean architecture patterns and service registration

using AutoMapper;
using Mattin.Project.Core.Factories;
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

        // Get the solution root directory
        var currentDirectory = Directory.GetCurrentDirectory();
        var solutionDir = Path.GetFullPath(Path.Combine(currentDirectory, ".."));
        while (!File.Exists(Path.Combine(solutionDir, "Mattin.Project.sln")))
        {
            solutionDir = Path.GetFullPath(Path.Combine(solutionDir, ".."));
            if (solutionDir == Path.GetPathRoot(solutionDir))
            {
                throw new InvalidOperationException("Could not find solution root directory.");
            }
        }

        var dataDir = Path.Combine(solutionDir, "Data");
        Console.WriteLine($"Solution directory: {solutionDir}"); // Debug line

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
            cfg.AddProfile<ServiceMappingProfile>();
        });

        // Repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IProjectManagerRepository, ProjectManagerRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();

        // Services
        services.AddScoped<IProjectService>(sp => new ProjectService(
            sp.GetRequiredService<IProjectRepository>(),
            sp.GetRequiredService<IStatusRepository>(),
            sp.GetRequiredService<IMappingFactory>(),
            sp.GetRequiredService<ApplicationDbContext>()
        ));
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProjectManagerService, ProjectManagerService>();
        services.AddScoped<IServiceService, ServiceService>();

        // Factories
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IMappingFactory, MappingFactory>();

        return services;
    }
}
