// Main menu implementation enhanced with AI assistance for:
// - User interface design
// - Navigation system
// - Visual effects and animations
// - Menu structure
// - Help system integration
// - Color theme management

using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Presentation.Helpers;
using Mattin.Project.Presentation.Menus.Base;

namespace Mattin.Project.Presentation.Menus;

/// <summary>
/// Main entry point for the application's user interface.
/// Provides access to all major functionality through an
/// intuitive menu system with visual enhancements.
/// </summary>
public class MainMenu : BaseMenu
{
    private readonly ProjectMenu _projectMenu;
    private readonly ClientMenu _clientMenu;

    public MainMenu(IServiceFactory serviceFactory)
        : base(serviceFactory)
    {
        _projectMenu = new ProjectMenu(serviceFactory);
        _clientMenu = new ClientMenu(serviceFactory);
    }

    public override async Task ShowAsync()
    {
        var running = true;
        while (running)
        {
            Console.Clear();
            LogoHelper.DisplayLogo();

            var options = new[]
            {
                "Client Management",
                "Project Management",
                "✨ Surprise!",
                "Exit",
            };

            Console.WriteLine("\nUse arrow keys to navigate and Enter to select");

            var choice = _menuHelper.ShowMenu(options, itemColor: ConsoleColor.Cyan);

            switch (choice)
            {
                case 0:
                    await _clientMenu.ShowAsync();
                    break;
                case 1:
                    await _projectMenu.ShowAsync();
                    break;
                case 2:
                    LogoHelper.DisplayMatrixEffect();
                    break;
                case 3:
                    running = false;
                    break;
            }
        }
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
