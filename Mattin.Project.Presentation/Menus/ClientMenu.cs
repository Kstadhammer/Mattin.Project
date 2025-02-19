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
        DisplayHeader("All Clients");
        var clients = await _clientService.GetAllAsync();

        if (!clients.Any())
        {
            Console.WriteLine("No clients found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            return;
        }

        foreach (var client in clients)
        {
            Console.WriteLine($"ID: {client.Id}");
            Console.WriteLine($"Name: {client.Name}");
            Console.WriteLine($"Email: {client.Email}");
            Console.WriteLine($"Phone: {client.PhoneNumber}");
            Console.WriteLine($"Address: {client.Address ?? "N/A"}");
            Console.WriteLine($"Active Projects: {client.ActiveProjectsCount}");
            Console.WriteLine($"Total Project Value: {client.FormattedTotalProjectValue}");
            Console.WriteLine("------------");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
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

            Console.WriteLine("Available Clients:");
            foreach (var client in clients)
            {
                Console.WriteLine($"{client.Id}: {client.Name}");
            }

            var clientId = int.Parse(_menuHelper.GetUserInput("Enter Client ID to edit"));
            var existingClient = clients.FirstOrDefault(c => c.Id == clientId);
            if (existingClient == null)
            {
                DisplayError("Invalid client ID.");
                return;
            }

            var dto = new UpdateClientDto
            {
                Id = clientId,
                Name = _menuHelper.GetUserInput("Client Name", true) ?? existingClient.Name,
                Email = _menuHelper.GetUserInput("Email", true) ?? existingClient.Email,
                PhoneNumber =
                    _menuHelper.GetUserInput("Phone Number", true) ?? existingClient.PhoneNumber,
                Address = _menuHelper.GetUserInput("Address", true) ?? existingClient.Address,
            };

            var updated = await _clientService.UpdateAsync(dto);
            DisplaySuccess("Client updated successfully");
        }
        catch (Exception ex)
        {
            DisplayError($"Failed to update client: {ex.Message}");
        }
    }
}
