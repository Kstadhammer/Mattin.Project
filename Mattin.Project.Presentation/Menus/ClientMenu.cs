using System.ComponentModel.DataAnnotations;
using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Presentation.Menus.Base;

namespace Mattin.Project.Presentation.Menus;

public class ClientMenu : BaseMenu
{
    private readonly IClientService _clientService;

    public ClientMenu(IServiceFactory serviceFactory)
        : base(serviceFactory)
    {
        _clientService = serviceFactory.CreateClientService();
    }

    public override async Task ShowAsync()
    {
        var running = true;
        while (running)
        {
            DisplayHeader("Client Management");
            var options = new[]
            {
                "List All Clients",
                "Create New Client",
                "Edit Client",
                "Back to Main Menu",
            };

            var choice = _menuHelper.ShowMenu(options);
            switch (choice)
            {
                case 0:
                    await ListClientsAsync();
                    break;
                case 1:
                    await CreateClientAsync();
                    break;
                case 2:
                    await EditClientAsync();
                    break;
                case 3:
                    running = false;
                    break;
            }
        }
    }

    private async Task ListClientsAsync()
    {
        while (true)
        {
            try
            {
                DisplayHeader("All Clients");
                var clients = await _clientService.GetAllAsync();

                if (!clients.Any())
                {
                    Console.WriteLine("No clients found.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    return;
                }

                // Display clients as selectable items
                var selectedClient = _menuHelper.SelectFromList(
                    "Clients",
                    clients,
                    c =>
                        $"Client: {c.Name}\n"
                        + $"  Email: {c.Email}\n"
                        + $"  Phone: {c.PhoneNumber}\n"
                        + $"  Address: {c.Address ?? "N/A"}\n"
                        + $"  Active Projects: {c.ActiveProjectsCount}\n"
                        + $"  Total Project Value: {c.FormattedTotalProjectValue}"
                );

                // Show edit options for the selected client
                var editOptions = new[]
                {
                    "Edit Name",
                    "Edit Email",
                    "Edit Phone Number",
                    "Edit Address",
                    "Back to Clients List",
                    "Exit to Menu",
                };

                var choice = _menuHelper.ShowMenu(editOptions);

                var dto = new UpdateClientDto
                {
                    Id = selectedClient.Id,
                    Name = selectedClient.Name,
                    Email = selectedClient.Email,
                    PhoneNumber = selectedClient.PhoneNumber,
                    Address = selectedClient.Address,
                };

                var updated = false;
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
                            updated = true;
                        }
                        break;
                    case 1: // Email
                        var newEmail = _menuHelper.GetUserInput("New Email", true);
                        if (!string.IsNullOrWhiteSpace(newEmail))
                        {
                            if (!IsValidEmail(newEmail))
                            {
                                DisplayError("Please enter a valid email address.");
                                continue;
                            }
                            if (newEmail.Length > 100)
                            {
                                DisplayError("Email cannot be longer than 100 characters.");
                                continue;
                            }
                            dto.Email = newEmail;
                            updated = true;
                        }
                        break;
                    case 2: // Phone Number
                        var newPhone = _menuHelper.GetUserInput("New Phone Number", true);
                        if (!string.IsNullOrWhiteSpace(newPhone))
                        {
                            if (!IsValidPhoneNumber(newPhone))
                            {
                                DisplayError("Please enter a valid phone number.");
                                continue;
                            }
                            if (newPhone.Length > 20)
                            {
                                DisplayError("Phone number cannot be longer than 20 characters.");
                                continue;
                            }
                            dto.PhoneNumber = newPhone;
                            updated = true;
                        }
                        break;
                    case 3: // Address
                        var newAddress = _menuHelper.GetUserInput("New Address", true);
                        if (newAddress?.Length > 200)
                        {
                            DisplayError("Address cannot be longer than 200 characters.");
                            continue;
                        }
                        dto.Address = newAddress;
                        updated = true;
                        break;
                    case 4: // Back to list
                        continue;
                    case 5: // Exit
                        return;
                }

                if (updated) // Only update if changes were made
                {
                    await _clientService.UpdateAsync(dto);
                    DisplaySuccess("Client updated successfully");
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Failed to update client: {ex.Message}");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var attribute = new EmailAddressAttribute();
            return attribute.IsValid(email);
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        try
        {
            var attribute = new PhoneAttribute();
            return attribute.IsValid(phoneNumber);
        }
        catch
        {
            return false;
        }
    }

    private async Task CreateClientAsync()
    {
        DisplayHeader("Create New Client");

        try
        {
            var dto = new CreateClientDto
            {
                Name = _menuHelper.GetUserInput("Client Name"),
                Email = _menuHelper.GetUserInput("Email"),
                PhoneNumber = _menuHelper.GetUserInput("Phone Number"),
                Address = _menuHelper.GetUserInput("Address (optional)", true),
            };

            var client = await _clientService.CreateAsync(dto);
            DisplaySuccess($"Client {client.Name} created successfully");
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to create client: {ex.Message}");
        }
    }

    private async Task EditClientAsync()
    {
        DisplayHeader("Edit Client");

        try
        {
            var clients = await _clientService.GetAllAsync();
            if (!clients.Any())
            {
                DisplayError("No clients available to edit.");
                return;
            }

            var selectedClient = _menuHelper.SelectFromList(
                "Clients",
                clients,
                c => $"{c.Name} ({c.Email})"
            );

            var dto = new UpdateClientDto
            {
                Id = selectedClient.Id,
                Name = selectedClient.Name,
                Email = selectedClient.Email,
                PhoneNumber = selectedClient.PhoneNumber,
                Address = selectedClient.Address,
            };

            var updated = false;

            // Name
            var newName = _menuHelper.GetUserInput(
                "Client Name (press Enter to keep current)",
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
                updated = true;
            }

            // Email
            var newEmail = _menuHelper.GetUserInput("Email (press Enter to keep current)", true);
            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                if (!IsValidEmail(newEmail))
                {
                    DisplayError("Please enter a valid email address.");
                    return;
                }
                if (newEmail.Length > 100)
                {
                    DisplayError("Email cannot be longer than 100 characters.");
                    return;
                }
                dto.Email = newEmail;
                updated = true;
            }

            // Phone
            var newPhone = _menuHelper.GetUserInput(
                "Phone Number (press Enter to keep current)",
                true
            );
            if (!string.IsNullOrWhiteSpace(newPhone))
            {
                if (!IsValidPhoneNumber(newPhone))
                {
                    DisplayError("Please enter a valid phone number.");
                    return;
                }
                if (newPhone.Length > 20)
                {
                    DisplayError("Phone number cannot be longer than 20 characters.");
                    return;
                }
                dto.PhoneNumber = newPhone;
                updated = true;
            }

            // Address
            var newAddress = _menuHelper.GetUserInput(
                "Address (press Enter to keep current)",
                true
            );
            if (newAddress != null)
            {
                if (newAddress.Length > 200)
                {
                    DisplayError("Address cannot be longer than 200 characters.");
                    return;
                }
                dto.Address = newAddress;
                updated = true;
            }

            if (updated)
            {
                await _clientService.UpdateAsync(dto);
                DisplaySuccess("Client updated successfully");
            }
            else
            {
                DisplaySuccess("No changes were made");
            }
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to update client: {ex.Message}");
        }
    }
}
