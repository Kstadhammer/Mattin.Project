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
                var clientsResult = await _clientService.GetAllAsync();
                if (clientsResult.IsFailure)
                {
                    DisplayError(clientsResult.Error);
                    return;
                }

                var clients = clientsResult.Value;
                if (!clients.Any())
                {
                    Console.WriteLine("No clients found.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    return;
                }

                // Add a null entry for "Back to Main Menu"
                var clientsList = clients.ToList();
                clientsList.Add(null!);

                var selectedClient = _menuHelper.SelectFromList(
                    "Clients",
                    clientsList,
                    c =>
                        c == null
                            ? "Back to Main Menu"
                            : $"Client: {c.Name}\n"
                                + $"  Email: {c.Email}\n"
                                + $"  Phone: {c.PhoneNumber}\n"
                                + $"  Address: {c.Address ?? "N/A"}\n"
                                + $"  Active Projects: {c.ActiveProjectsCount}\n"
                                + $"  Total Project Value: {c.FormattedTotalProjectValue}\n",
                    ConsoleColor.Yellow
                );

                if (selectedClient == null)
                    return;

                var editOptions = new[]
                {
                    "Edit Name",
                    "Edit Email",
                    "Edit Phone Number",
                    "Edit Address",
                    "Back to Clients List",
                };

                DisplayHeader($"Editing Client: {selectedClient.Name}");
                var choice = _menuHelper.ShowMenu(editOptions, itemColor: ConsoleColor.Green);

                if (choice == 4) // Back to list
                    continue;

                var dto = new UpdateClientDto
                {
                    Id = selectedClient.Id,
                    Name = selectedClient.Name,
                    Email = selectedClient.Email,
                    PhoneNumber = selectedClient.PhoneNumber,
                    Address = selectedClient.Address,
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
                                    DisplayError(
                                        "Phone number cannot be longer than 20 characters."
                                    );
                                    continue;
                                }
                                dto.PhoneNumber = newPhone;
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
                            break;
                    }

                    var updateResult = await _clientService.UpdateAsync(dto);
                    if (updateResult.IsFailure)
                        DisplayError(updateResult.Error);
                    else
                        DisplaySuccess("Client updated successfully");
                }
                catch (Exception ex)
                {
                    DisplayError($"Failed to update client: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Failed to update client: {ex.Message}");
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

            var createResult = await _clientService.CreateAsync(dto);
            if (createResult.IsFailure)
                DisplayError(createResult.Error);
            else
                DisplaySuccess($"Client {createResult.Value.Name} created successfully");
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
            var clientsResult = await _clientService.GetAllAsync();
            if (clientsResult.IsFailure)
            {
                DisplayError(clientsResult.Error);
                return;
            }

            var clients = clientsResult.Value;
            if (!clients.Any())
            {
                DisplayError("No clients available to edit.");
                return;
            }

            // Add a null entry for "Back to Main Menu"
            var clientsList = clients.ToList();
            clientsList.Add(null!);

            var selectedClient = _menuHelper.SelectFromList(
                "Clients",
                clientsList,
                c =>
                    c == null
                        ? "Back to Main Menu"
                        : $"Client: {c.Name}\n"
                            + $"  Email: {c.Email}\n"
                            + $"  Phone: {c.PhoneNumber}\n"
                            + $"  Address: {c.Address ?? "N/A"}\n"
                            + $"  Active Projects: {c.ActiveProjectsCount}\n"
                            + $"  Total Project Value: {c.FormattedTotalProjectValue}\n",
                ConsoleColor.Yellow
            );

            if (selectedClient == null)
                return;

            var editOptions = new[]
            {
                "Edit Name",
                "Edit Email",
                "Edit Phone Number",
                "Edit Address",
                "Back to Clients List",
            };

            DisplayHeader($"Editing Client: {selectedClient.Name}");
            var choice = _menuHelper.ShowMenu(editOptions, itemColor: ConsoleColor.Green);

            if (choice == 4) // Back to list
                return;

            var dto = new UpdateClientDto
            {
                Id = selectedClient.Id,
                Name = selectedClient.Name,
                Email = selectedClient.Email,
                PhoneNumber = selectedClient.PhoneNumber,
                Address = selectedClient.Address,
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
                    case 1: // Email
                        var newEmail = _menuHelper.GetUserInput(
                            "New Email (press Enter to keep current)",
                            true
                        );
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
                        }
                        break;
                    case 2: // Phone Number
                        var newPhone = _menuHelper.GetUserInput(
                            "New Phone Number (press Enter to keep current)",
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
                        }
                        break;
                    case 3: // Address
                        var newAddress = _menuHelper.GetUserInput(
                            "New Address (press Enter to keep current)",
                            true
                        );
                        if (newAddress?.Length > 200)
                        {
                            DisplayError("Address cannot be longer than 200 characters.");
                            return;
                        }
                        dto.Address = newAddress;
                        break;
                }

                if (
                    dto.Name != selectedClient.Name
                    || dto.Email != selectedClient.Email
                    || dto.PhoneNumber != selectedClient.PhoneNumber
                    || dto.Address != selectedClient.Address
                )
                {
                    var updateResult = await _clientService.UpdateAsync(dto);
                    if (updateResult.IsFailure)
                        DisplayError(updateResult.Error);
                    else
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
        catch (Exception ex)
        {
            DisplayError($"Failed to update client: {ex.Message}");
        }
    }
}
