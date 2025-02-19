using Mattin.Project.Core.Interfaces.Factories;
using Mattin.Project.Presentation.Helpers;

namespace Mattin.Project.Presentation.Menus.Base;

public abstract class BaseMenu
{
    protected readonly MenuHelper _menuHelper;
    protected readonly IServiceFactory _serviceFactory;

    protected BaseMenu(IServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
        _menuHelper = new MenuHelper();
    }

    public abstract Task ShowAsync();

    protected void DisplayHeader(string title)
    {
        Console.Clear();
        Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
        Console.WriteLine(title.PadLeft((Console.WindowWidth + title.Length) / 2));
        Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
        Console.WriteLine();
    }

    protected async Task<bool> ConfirmActionAsync(string message)
    {
        Console.WriteLine($"\n{message} (Y/N)");
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Y)
                return true;
            if (key.Key == ConsoleKey.N)
                return false;
        }
    }

    protected void DisplayError(string message)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nError: {message}");
        Console.ForegroundColor = currentColor;
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }

    protected void DisplaySuccess(string message)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n{message}");
        Console.ForegroundColor = currentColor;
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
}
