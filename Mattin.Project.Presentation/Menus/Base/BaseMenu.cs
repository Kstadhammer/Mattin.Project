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
        Console.WriteLine($"\n{message}");
        var options = new[] { "Yes", "No" };
        var selectedIndex = 1; // Default to "No" for safety

        ConsoleKey keyPressed;
        do
        {
            Console.CursorVisible = false;

            // Display options
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                Console.Write($"[{options[i]}] ");
                Console.ResetColor();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.LeftArrow || keyPressed == ConsoleKey.RightArrow)
            {
                selectedIndex = 1 - selectedIndex; // Toggle between 0 and 1
            }
            else if (keyPressed == ConsoleKey.Y)
            {
                selectedIndex = 0;
                keyPressed = ConsoleKey.Enter;
            }
            else if (keyPressed == ConsoleKey.N)
            {
                selectedIndex = 1;
                keyPressed = ConsoleKey.Enter;
            }

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', options.Sum(o => o.Length + 3))); // Clear the line
            Console.SetCursorPosition(0, Console.CursorTop);
        } while (keyPressed != ConsoleKey.Enter);

        Console.WriteLine(); // Move to next line
        Console.CursorVisible = true;
        return selectedIndex == 0;
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
