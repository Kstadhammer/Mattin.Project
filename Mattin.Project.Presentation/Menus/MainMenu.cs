using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Presentation.Helpers;
using Mattin.Project.Presentation.Menus.Base;

namespace Mattin.Project.Presentation.Menus;

public class MainMenu : BaseMenu
{
    public MainMenu(IServiceFactory serviceFactory)
        : base(serviceFactory) { }

    public override async Task ShowAsync()
    {
        var running = true;
        while (running)
        {
            Console.Clear();
            LogoHelper.DisplayLogo();

            var options = new[]
            {
                "📄 Project Management",
                "👥 Client Management",
                "❔ Help & Information",
                "❌ Exit Application",
            };

            var choice = _menuHelper.ShowMenu(options);

            switch (choice)
            {
                case 0:
                    await ShowProjectManagementAsync();
                    break;
                case 1:
                    await ShowClientManagementAsync();
                    break;
                case 2:
                    ShowHelp();
                    break;
                case 3:
                    if (await ConfirmActionAsync("Are you sure you want to exit?"))
                        running = false;
                    break;
            }
        }
    }

    private async Task ShowProjectManagementAsync()
    {
        Console.Clear();
        LogoHelper.DisplaySmallLogo();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n  Project Management");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("  Manage your projects, track status, and monitor progress.\n");
        Console.ResetColor();

        var projectMenu = new ProjectMenu(_serviceFactory);
        await projectMenu.ShowAsync();
    }

    private async Task ShowClientManagementAsync()
    {
        Console.Clear();
        LogoHelper.DisplaySmallLogo();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n  Client Management");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("  Manage your clients and their associated projects.\n");
        Console.ResetColor();

        var clientMenu = new ClientMenu(_serviceFactory);
        await clientMenu.ShowAsync();
    }

    private void ShowHelp()
    {
        Console.Clear();
        LogoHelper.DisplaySmallLogo();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n  Help & Information\n");
        Console.ResetColor();

        Console.WriteLine("  Navigation Controls:");
        Console.WriteLine("  ▲▼  Arrow keys - Move selection up/down");
        Console.WriteLine("  ↵   Enter - Select option");
        Console.WriteLine("  ⎋   Esc - Go back/Cancel\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  Project Management Features:");
        Console.ResetColor();
        Console.WriteLine("  • View all projects and their details");
        Console.WriteLine("  • Create new projects");
        Console.WriteLine("  • Edit existing projects");
        Console.WriteLine("  • Track project status and progress\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  Client Management Features:");
        Console.ResetColor();
        Console.WriteLine("  • View all clients and their details");
        Console.WriteLine("  • Create new clients");
        Console.WriteLine("  • Edit client information");
        Console.WriteLine("  • View client's projects and total value\n");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\n  Press any key to return to main menu...");
        Console.ResetColor();
        Console.ReadKey(true);
    }
}
