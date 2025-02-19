using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Core.Models.DTOs.Service;
using Mattin.Project.Presentation.Menus.Base;

namespace Mattin.Project.Presentation.Menus;

public class ServiceMenu : BaseMenu
{
    private readonly IServiceService _serviceService;

    public ServiceMenu(IServiceFactory serviceFactory)
        : base(serviceFactory)
    {
        _serviceService = serviceFactory.CreateServiceService();
    }

    public override async Task ShowAsync()
    {
        var running = true;
        while (running)
        {
            DisplayHeader("Service Management");
            var options = new[]
            {
                "List All Services",
                "Create New Service",
                "Edit Service",
                "Back to Main Menu",
            };

            var choice = _menuHelper.ShowMenu(options);
            switch (choice)
            {
                case 0:
                    await ListServicesAsync();
                    break;
                case 1:
                    await CreateServiceAsync();
                    break;
                case 2:
                    await EditServiceAsync();
                    break;
                case 3:
                    running = false;
                    break;
            }
        }
    }

    private async Task ListServicesAsync()
    {
        while (true)
        {
            try
            {
                DisplayHeader("All Services");
                var servicesResult = await _serviceService.GetAllAsync();
                if (servicesResult.IsFailure)
                {
                    DisplayError(servicesResult.Error);
                    return;
                }

                var services = servicesResult.Value;
                if (!services.Any())
                {
                    Console.WriteLine("No services found.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    return;
                }

                // Add a null entry for "Back to Main Menu"
                var servicesList = services.ToList();
                servicesList.Add(null!);

                var selectedService = _menuHelper.SelectFromList(
                    "Services",
                    servicesList,
                    s =>
                        s == null
                            ? "Back to Main Menu"
                            : $"Service: {s.Name}\n"
                                + $"  Description: {s.Description}\n"
                                + $"  Base Price: {s.FormattedBasePrice}\n"
                                + $"  Hourly Rate: {s.FormattedHourlyRate}\n"
                                + $"  Category: {s.Category ?? "N/A"}\n"
                                + $"  Status: {(s.IsActive ? "Active" : "Inactive")}\n"
                                + $"  Active Projects: {s.ActiveProjectsCount}\n",
                    ConsoleColor.Yellow
                );

                if (selectedService == null)
                    return;

                var editOptions = new[]
                {
                    "Edit Name",
                    "Edit Description",
                    "Edit Base Price",
                    "Edit Hourly Rate",
                    "Edit Category",
                    "Toggle Active Status",
                    "Back to Services List",
                };

                DisplayHeader($"Editing Service: {selectedService.Name}");
                var choice = _menuHelper.ShowMenu(editOptions, itemColor: ConsoleColor.Green);

                if (choice == 6) // Back to list
                    continue;

                var dto = new UpdateServiceDto
                {
                    Id = selectedService.Id,
                    Name = selectedService.Name,
                    Description = selectedService.Description,
                    BasePrice = selectedService.BasePrice,
                    HourlyRate = selectedService.HourlyRate,
                    Category = selectedService.Category,
                    IsActive = selectedService.IsActive,
                };

                try
                {
                    switch (choice)
                    {
                        case 0: // Name
                            var newName = _menuHelper.GetUserInput("New Name", true);
                            if (!string.IsNullOrWhiteSpace(newName))
                            {
                                if (newName.Length > 100)
                                {
                                    DisplayError("Name cannot be longer than 100 characters.");
                                    continue;
                                }
                                dto.Name = newName;
                            }
                            break;
                        case 1: // Description
                            var newDescription = _menuHelper.GetUserInput("New Description", true);
                            if (!string.IsNullOrWhiteSpace(newDescription))
                            {
                                if (newDescription.Length > 500)
                                {
                                    DisplayError(
                                        "Description cannot be longer than 500 characters."
                                    );
                                    continue;
                                }
                                dto.Description = newDescription;
                            }
                            break;
                        case 2: // Base Price
                            dto.BasePrice = _menuHelper.GetDecimalInput("New Base Price (kr)");
                            break;
                        case 3: // Hourly Rate
                            dto.HourlyRate = _menuHelper.GetDecimalInput("New Hourly Rate (kr)");
                            break;
                        case 4: // Category
                            var newCategory = _menuHelper.GetUserInput("New Category", true);
                            if (newCategory?.Length > 50)
                            {
                                DisplayError("Category cannot be longer than 50 characters.");
                                continue;
                            }
                            dto.Category = newCategory;
                            break;
                        case 5: // Toggle Active Status
                            dto.IsActive = !dto.IsActive;
                            break;
                    }

                    var updateResult = await _serviceService.UpdateAsync(dto);
                    if (updateResult.IsFailure)
                        DisplayError(updateResult.Error);
                    else
                        DisplaySuccess("Service updated successfully");
                }
                catch (Exception ex)
                {
                    DisplayError($"Failed to update service: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Failed to update service: {ex.Message}");
            }
        }
    }

    private async Task CreateServiceAsync()
    {
        DisplayHeader("Create New Service");

        try
        {
            var dto = new CreateServiceDto
            {
                Name = _menuHelper.GetUserInput("Service Name"),
                Description = _menuHelper.GetUserInput("Description"),
                BasePrice = _menuHelper.GetDecimalInput("Base Price (kr)"),
                HourlyRate = _menuHelper.GetDecimalInput("Hourly Rate (kr)"),
                Category = _menuHelper.GetUserInput("Category (optional)", true),
            };

            var createResult = await _serviceService.CreateAsync(dto);
            if (createResult.IsFailure)
                DisplayError(createResult.Error);
            else
                DisplaySuccess($"Service {createResult.Value.Name} created successfully");
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to create service: {ex.Message}");
        }
    }

    private async Task EditServiceAsync()
    {
        DisplayHeader("Edit Service");

        try
        {
            var servicesResult = await _serviceService.GetAllAsync();
            if (servicesResult.IsFailure)
            {
                DisplayError(servicesResult.Error);
                return;
            }

            var services = servicesResult.Value;
            if (!services.Any())
            {
                DisplayError("No services available to edit.");
                return;
            }

            // Add a null entry for "Back to Main Menu"
            var servicesList = services.ToList();
            servicesList.Add(null!);

            var selectedService = _menuHelper.SelectFromList(
                "Services",
                servicesList,
                s =>
                    s == null
                        ? "Back to Main Menu"
                        : $"Service: {s.Name}\n"
                            + $"  Description: {s.Description}\n"
                            + $"  Base Price: {s.FormattedBasePrice}\n"
                            + $"  Hourly Rate: {s.FormattedHourlyRate}\n"
                            + $"  Category: {s.Category ?? "N/A"}\n"
                            + $"  Status: {(s.IsActive ? "Active" : "Inactive")}\n"
                            + $"  Active Projects: {s.ActiveProjectsCount}\n",
                ConsoleColor.Yellow
            );

            if (selectedService == null)
                return;

            var editOptions = new[]
            {
                "Edit Name",
                "Edit Description",
                "Edit Base Price",
                "Edit Hourly Rate",
                "Edit Category",
                "Toggle Active Status",
                "Back to Services List",
            };

            DisplayHeader($"Editing Service: {selectedService.Name}");
            var choice = _menuHelper.ShowMenu(editOptions, itemColor: ConsoleColor.Green);

            if (choice == 6) // Back to list
                return;

            var dto = new UpdateServiceDto
            {
                Id = selectedService.Id,
                Name = selectedService.Name,
                Description = selectedService.Description,
                BasePrice = selectedService.BasePrice,
                HourlyRate = selectedService.HourlyRate,
                Category = selectedService.Category,
                IsActive = selectedService.IsActive,
            };

            try
            {
                switch (choice)
                {
                    case 0: // Name
                        var newName = _menuHelper.GetUserInput(
                            "New Name (press Enter to keep current)",
                            true
                        );
                        if (!string.IsNullOrWhiteSpace(newName))
                        {
                            if (newName.Length > 100)
                            {
                                DisplayError("Name cannot be longer than 100 characters.");
                                return;
                            }
                            dto.Name = newName;
                        }
                        break;
                    case 1: // Description
                        var newDescription = _menuHelper.GetUserInput(
                            "New Description (press Enter to keep current)",
                            true
                        );
                        if (!string.IsNullOrWhiteSpace(newDescription))
                        {
                            if (newDescription.Length > 500)
                            {
                                DisplayError("Description cannot be longer than 500 characters.");
                                return;
                            }
                            dto.Description = newDescription;
                        }
                        break;
                    case 2: // Base Price
                        dto.BasePrice = _menuHelper.GetDecimalInput("New Base Price (kr)");
                        break;
                    case 3: // Hourly Rate
                        dto.HourlyRate = _menuHelper.GetDecimalInput("New Hourly Rate (kr)");
                        break;
                    case 4: // Category
                        var newCategory = _menuHelper.GetUserInput(
                            "New Category (press Enter to keep current)",
                            true
                        );
                        if (newCategory?.Length > 50)
                        {
                            DisplayError("Category cannot be longer than 50 characters.");
                            return;
                        }
                        dto.Category = newCategory;
                        break;
                    case 5: // Toggle Active Status
                        dto.IsActive = !dto.IsActive;
                        break;
                }

                if (
                    dto.Name != selectedService.Name
                    || dto.Description != selectedService.Description
                    || dto.BasePrice != selectedService.BasePrice
                    || dto.HourlyRate != selectedService.HourlyRate
                    || dto.Category != selectedService.Category
                    || dto.IsActive != selectedService.IsActive
                )
                {
                    var updateResult = await _serviceService.UpdateAsync(dto);
                    if (updateResult.IsFailure)
                        DisplayError(updateResult.Error);
                    else
                        DisplaySuccess("Service updated successfully");
                }
                else
                {
                    DisplaySuccess("No changes were made");
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Failed to update service: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to update service: {ex.Message}");
        }
    }
}
