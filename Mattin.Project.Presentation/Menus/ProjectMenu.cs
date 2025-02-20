// Project menu implementation enhanced with AI assistance for:
// - Interactive menu navigation
// - Project data management
// - Input validation and error handling
// - Color-coded user interface
// - Status management
// - Budget calculations

using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Core.Models.Entities;
using Mattin.Project.Presentation.Menus.Base;

namespace Mattin.Project.Presentation.Menus;

/// <summary>
/// Handles the project management interface, providing options for
/// creating, viewing, and editing projects with a user-friendly
/// color-coded menu system.
/// </summary>
public class ProjectMenu(IServiceFactory serviceFactory) : BaseMenu(serviceFactory)
{
    private readonly IProjectService _projectService = serviceFactory.CreateProjectService();
    private readonly IClientService _clientService = serviceFactory.CreateClientService();
    private readonly IProjectManagerService _projectManagerService =
        serviceFactory.CreateProjectManagerService();
    private readonly IServiceService _serviceService = serviceFactory.CreateServiceService();

    public override async Task ShowAsync()
    {
        var running = true;
        while (running)
        {
            DisplayHeader("Project Management");
            var options = new[] { "List/Edit Projects", "Create New Project", "Back to Main Menu" };

            var choice = _menuHelper.ShowMenu(options, itemColor: ConsoleColor.Yellow);
            switch (choice)
            {
                case 0:
                    await ListProjectsAsync();
                    break;
                case 1:
                    await CreateProjectAsync();
                    break;
                case 2:
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
            var projectsResult = await _projectService.GetAllAsync();
            if (projectsResult.IsFailure)
            {
                DisplayError(projectsResult.Error);
                return;
            }

            var projects = projectsResult.Value;
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
                            + $"  Budget: {p.FormattedTotalPrice}\n",
                ConsoleColor.Yellow
            );

            if (selectedProject == null)
                return;

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
            };

            var choice = _menuHelper.ShowMenu(editOptions, itemColor: ConsoleColor.Green);

            if (choice == 7) // Back to list
                continue;

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
                            status => status,
                            ConsoleColor.DarkYellow
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
                }

                if (choice < 7) // Only update if an edit option was selected
                {
                    var updateResult = await _projectService.UpdateAsync(dto);
                    if (updateResult.IsFailure)
                        DisplayError(updateResult.Error);
                    else
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
            var clientsResult = await _clientService.GetAllAsync();
            if (clientsResult.IsFailure)
            {
                DisplayError(clientsResult.Error);
                return;
            }

            // Add a null entry for "Back to Main Menu"
            var clientsList = clientsResult.Value.ToList();
            clientsList.Add(null!);

            var selectedClient = _menuHelper.SelectFromList(
                "Clients",
                clientsList,
                client => client == null ? "Back to Main Menu" : $"{client.Id}: {client.Name}",
                ConsoleColor.Green
            );

            if (selectedClient == null)
                return;

            // Get and select project manager
            var managersResult = await _projectManagerService.GetAllAsync();
            if (managersResult.IsFailure)
            {
                DisplayError(managersResult.Error);
                return;
            }

            var selectedManager = _menuHelper.SelectFromList(
                "Project Managers",
                managersResult.Value,
                pm => $"{pm.Id}: {pm.Name} ({pm.Department})",
                ConsoleColor.Magenta
            );

            // Get and select service
            var servicesResult = await _serviceService.GetActiveServicesAsync();
            if (servicesResult.IsFailure)
            {
                DisplayError(servicesResult.Error);
                return;
            }

            var selectedService = _menuHelper.SelectFromList(
                "Services",
                servicesResult.Value,
                service =>
                    $"{service.Id}: {service.Name} - {service.FormattedBasePrice} (Rate: {service.FormattedHourlyRate})",
                ConsoleColor.Yellow
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

            // Use service's hourly rate if available
            var hourlyRate =
                selectedService.HourlyRate > 0
                    ? selectedService.HourlyRate
                    : _menuHelper.GetDecimalInput("Hourly Rate (kr)");

            // Calculate suggested total price based on service base price and duration
            var durationDays = (endDate - startDate).Days;
            var suggestedTotalPrice = selectedService.BasePrice + (hourlyRate * durationDays * 8);
            Console.WriteLine(
                $"\nSuggested total price based on duration and service base price: {suggestedTotalPrice:C}"
            );
            var totalPrice = _menuHelper.GetDecimalInput(
                "Total Price (kr)",
                minValue: selectedService.BasePrice
            );

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
                ServiceId = selectedService.Id,
            };

            var createResult = await _projectService.CreateAsync(dto);
            if (createResult.IsFailure)
                DisplayError(createResult.Error);
            else
                DisplaySuccess(
                    $"Project created successfully with number: {createResult.Value.ProjectNumber}"
                );
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to create project: {ex.Message}");
        }
    }
}
