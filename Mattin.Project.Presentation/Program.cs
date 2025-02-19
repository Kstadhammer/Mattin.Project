// See https://aka.ms/new-console-template for more information

using System.IO;
using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Infrastructure;
using Mattin.Project.Presentation.Menus;
using Microsoft.Extensions.DependencyInjection;

// Configure console window
Console.Title = "Mattin-Lassei Project Management System";
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CursorVisible = false;

try
{
    var services = new ServiceCollection();
    services.AddInfrastructure(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mattinproject.db")
    );
    var serviceProvider = services.BuildServiceProvider();

    var serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
    var mainMenu = new MainMenu(serviceFactory);
    await mainMenu.ShowAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\nAn error occurred: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
    }
    Console.ResetColor();
    Console.WriteLine("\nPress any key to exit...");
    Console.ReadKey(true);
}
