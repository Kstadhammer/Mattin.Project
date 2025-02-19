using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Presentation.Menus.Base;

namespace Mattin.Project.Presentation.Menus;

public class ProjectMenu : BaseMenu
{
    private readonly IProjectService _projectService;
    private readonly IClientService _clientService;
    private readonly IProjectManagerService _projectManagerService;

    public ProjectMenu(IServiceFactory serviceFactory)
        : base(serviceFactory)
    {
        _projectService = serviceFactory.CreateProjectService();
        _clientService = serviceFactory.CreateClientService();
        _projectManagerService = serviceFactory.CreateProjectManagerService();
    }

    public override async Task ShowAsync()
    {
        var running = true;
        while (running)
        {
            DisplayHeader("Project Management");
            var options = new[]
            {
                "List All Projects",
                "Create New Project",
                "Edit Project",
                "Back to Main Menu",
            };

            var choice = _menuHelper.ShowMenu(options);
            switch (choice)
            {
                case 0:
                    await ListProjectsAsync();
                    break;
                case 1:
                    await CreateProjectAsync();
                    break;
                case 2:
                    await EditProjectAsync();
                    break;
                case 3:
                    running = false;
                    break;
            }
        }
    }

    private async Task ListProjectsAsync()
    {
        DisplayHeader("All Projects");
        var projects = await _projectService.GetAllAsync();

        if (!projects.Any())
        {
            Console.WriteLine("No projects found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            return;
        }

        foreach (var project in projects)
        {
            Console.WriteLine($"Project Number: {project.ProjectNumber}");
            Console.WriteLine($"Title: {project.Title}");
            Console.WriteLine($"Description: {project.Description}");
            Console.WriteLine($"Client: {project.Client?.Name ?? "N/A"}");
            Console.WriteLine($"Status: {project.Status ?? "N/A"}");

            if (project.ProjectManager != null)
            {
                Console.WriteLine($"Project Manager: {project.ProjectManager.Name}");
                Console.WriteLine($"Department: {project.ProjectManager.Department}");
                Console.WriteLine(
                    $"Contact: {project.ProjectManager.Email} / {project.ProjectManager.PhoneNumber}"
                );
            }
            else
            {
                Console.WriteLine("Project Manager: N/A");
                Console.WriteLine("Department: N/A");
                Console.WriteLine("Contact: N/A");
            }

            Console.WriteLine($"Start Date: {project.StartDate:yyyy-MM-dd}");
            Console.WriteLine(
                $"End Date: {(project.EndDate.HasValue ? project.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")}"
            );
            Console.WriteLine($"Hourly Rate: {project.FormattedHourlyRate}");
            Console.WriteLine($"Total Price: {project.FormattedTotalPrice}");
            Console.WriteLine("------------");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }

    private async Task CreateProjectAsync()
    {
        DisplayHeader("Create New Project");

        try
        {
            // Get and select client
            var clients = await _clientService.GetAllAsync();
            var selectedClient = _menuHelper.SelectFromList(
                "Clients",
                clients,
                client => $"{client.Id}: {client.Name}"
            );

            // Get and select project manager
            var projectManagers = await _projectManagerService.GetAllAsync();
            var selectedManager = _menuHelper.SelectFromList(
                "Project Managers",
                projectManagers,
                pm => $"{pm.Id}: {pm.Name} ({pm.Department})"
            );

            var dto = new CreateProjectDto
            {
                Title = _menuHelper.GetUserInput("Project Title"),
                Description = _menuHelper.GetUserInput("Description"),
                StartDate = _menuHelper.GetDateInput("Start Date"),
                EndDate = _menuHelper.GetDateInput("End Date"),
                ProjectManagerId = selectedManager.Id,
                HourlyRate = _menuHelper.GetDecimalInput("Hourly Rate (kr)"),
                TotalPrice = _menuHelper.GetDecimalInput("Total Price (kr)", minValue: 0),
                ClientId = selectedClient.Id,
            };

            var project = await _projectService.CreateAsync(dto);
            DisplaySuccess($"Project created successfully with number: {project.ProjectNumber}");
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to create project: {ex.Message}");
        }
    }

    private async Task EditProjectAsync()
    {
        DisplayHeader("Edit Project");

        try
        {
            var projects = await _projectService.GetAllAsync();
            var selectedProject = _menuHelper.SelectFromList(
                "Projects",
                projects,
                p => $"{p.Id}: {p.ProjectNumber} - {p.Title}"
            );

            var dto = new UpdateProjectDto
            {
                Id = selectedProject.Id,
                Title = _menuHelper.GetUserInput("Project Title", true) ?? selectedProject.Title,
                Description =
                    _menuHelper.GetUserInput("Description", true) ?? selectedProject.Description,
                Status =
                    _menuHelper.GetUserInput("Status (Ej påbörjat/Pågående/Avslutat)", true)
                    ?? selectedProject.Status,
                StartDate = _menuHelper.GetDateInput("Start Date"),
                EndDate = _menuHelper.GetDateInput("End Date"),
                ProjectManagerId = selectedProject.ProjectManagerId,
                HourlyRate = _menuHelper.GetDecimalInput("Hourly Rate (kr)"),
                TotalPrice = _menuHelper.GetDecimalInput("Total Price (kr)", minValue: 0),
                ClientId = selectedProject.ClientId,
            };

            var updated = await _projectService.UpdateAsync(dto);
            DisplaySuccess("Project updated successfully");
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to update project: {ex.Message}");
        }
    }
}
