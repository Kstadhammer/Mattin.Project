using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;
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

            var choice = _menuHelper.ShowMenu(options, itemColor: ConsoleColor.Cyan);
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
        while (true)
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

            // Display projects as selectable items with a "Back to Main Menu" option
            var projectsList = projects.ToList();
            projectsList.Add(null!); // Add a null entry for the "Back to Main Menu" option
            var selectedProject = _menuHelper.SelectFromList(
                "Projects",
                projectsList,
                p =>
                    p == null
                        ? "Back to Main Menu"
                        : $"Project: {p.ProjectNumber} - {p.Title}\n"
                            + $"  Client: {p.Client?.Name ?? "N/A"}\n"
                            + $"  Status: {p.Status}\n"
                            + $"  Manager: {p.ProjectManager?.Name ?? "N/A"}\n"
                            + $"  Timeline: {p.StartDate:yyyy-MM-dd} to {(p.EndDate.HasValue ? p.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")}\n"
                            + $"  Budget: {p.FormattedTotalPrice}",
                ConsoleColor.Cyan
            );

            // If "Back to Main Menu" was selected
            if (selectedProject == null)
            {
                return;
            }

            // Show edit options for the selected project
            var editOptions = new[]
            {
                "Edit Title",
                "Edit Description",
                "Edit Status",
                "Edit Start Date",
                "Edit End Date",
                "Edit Hourly Rate",
                "Edit Total Price",
                "Back to Projects List",
                "Exit to Menu",
            };

            var choice = _menuHelper.ShowMenu(editOptions, itemColor: ConsoleColor.Yellow);

            try
            {
                var dto = new UpdateProjectDto
                {
                    Id = selectedProject.Id,
                    Title = selectedProject.Title,
                    Description = selectedProject.Description,
                    Status = selectedProject.Status,
                    StartDate = selectedProject.StartDate,
                    EndDate = selectedProject.EndDate,
                    ProjectManagerId = selectedProject.ProjectManagerId,
                    HourlyRate = selectedProject.HourlyRate,
                    TotalPrice = selectedProject.TotalPrice,
                    ClientId = selectedProject.ClientId,
                };

                switch (choice)
                {
                    case 0: // Title
                        dto.Title = _menuHelper.GetUserInput("New Title", true) ?? dto.Title;
                        break;
                    case 1: // Description
                        dto.Description =
                            _menuHelper.GetUserInput("New Description", true) ?? dto.Description;
                        break;
                    case 2: // Status
                        var statuses = new[]
                        {
                            ProjectStatus.NotStarted,
                            ProjectStatus.InProgress,
                            ProjectStatus.Completed,
                        };
                        var selectedStatus = _menuHelper.SelectFromList(
                            "Statuses",
                            statuses,
                            status => status
                        );
                        dto.Status = selectedStatus;
                        break;
                    case 3: // Start Date
                        dto.StartDate = _menuHelper.GetDateInput("New Start Date");
                        break;
                    case 4: // End Date
                        var endDate = _menuHelper.GetDateInput("New End Date");
                        dto.EndDate = endDate;
                        break;
                    case 5: // Hourly Rate
                        dto.HourlyRate = _menuHelper.GetDecimalInput("New Hourly Rate (kr)");
                        break;
                    case 6: // Total Price
                        dto.TotalPrice = _menuHelper.GetDecimalInput("New Total Price (kr)");
                        break;
                    case 7: // Back to list
                        continue;
                    case 8: // Exit
                        return;
                }

                if (choice < 7) // Only update if an edit option was selected
                {
                    await _projectService.UpdateAsync(dto);
                    DisplaySuccess("Project updated successfully");
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Failed to update project: {ex.Message}");
            }
        }
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
                client => $"{client.Id}: {client.Name}",
                ConsoleColor.Green
            );

            // Get and select project manager
            var projectManagers = await _projectManagerService.GetAllAsync();
            var selectedManager = _menuHelper.SelectFromList(
                "Project Managers",
                projectManagers,
                pm => $"{pm.Id}: {pm.Name} ({pm.Department})",
                ConsoleColor.Magenta
            );

            // Select status
            var statuses = new[]
            {
                ProjectStatus.NotStarted,
                ProjectStatus.InProgress,
                ProjectStatus.Completed,
            };
            var selectedStatus = _menuHelper.SelectFromList(
                "Statuses",
                statuses,
                status => status,
                ConsoleColor.DarkYellow
            );

            // Get project details
            var title = _menuHelper.GetUserInput("Project Title");
            var description = _menuHelper.GetUserInput("Description");
            var startDate = _menuHelper.GetDateInput("Start Date");
            var endDate = _menuHelper.GetDateInput("End Date");
            var hourlyRate = _menuHelper.GetDecimalInput("Hourly Rate (kr)");
            var totalPrice = _menuHelper.GetDecimalInput("Total Price (kr)", minValue: 0);

            // Create the DTO with all required fields
            var dto = new CreateProjectDto
            {
                Title = title,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                ProjectManagerId = selectedManager.Id,
                HourlyRate = hourlyRate,
                TotalPrice = totalPrice,
                ClientId = selectedClient.Id,
                Status = selectedStatus,
            };

            // Create the project
            var project = await _projectService.CreateAsync(dto);
            DisplaySuccess($"Project created successfully with number: {project.ProjectNumber}");
        }
        catch (Exception ex)
        {
            // Get the innermost exception message for more details
            var message = ex.Message;
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                message = innerException.Message;
                innerException = innerException.InnerException;
            }
            DisplayError($"Failed to create project: {message}");
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
