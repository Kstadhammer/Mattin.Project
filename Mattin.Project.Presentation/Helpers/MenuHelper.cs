// Menu helper class developed with AI assistance for:
// - Arrow key navigation
// - List selection functionality
// - Input validation and formatting
// - Console display enhancements

namespace Mattin.Project.Presentation.Helpers;

public class MenuHelper
{
    public int ShowMenu(string[] options, int selectedIndex = 0, ConsoleColor? itemColor = null)
    {
        ConsoleKey keyPressed;
        do
        {
            Console.CursorVisible = false;

            // Clear the display area
            var currentTop = Console.CursorTop;
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(0, currentTop + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, currentTop);

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (itemColor.HasValue)
                {
                    Console.ResetColor();
                    Console.ForegroundColor = itemColor.Value;
                }

                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.UpArrow)
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = options.Length - 1;
                Console.SetCursorPosition(0, currentTop);
            }
            else if (keyPressed == ConsoleKey.DownArrow)
            {
                selectedIndex++;
                if (selectedIndex >= options.Length)
                    selectedIndex = 0;
                Console.SetCursorPosition(0, currentTop);
            }
        } while (keyPressed != ConsoleKey.Enter);

        Console.CursorVisible = true;
        return selectedIndex;
    }

    public T SelectFromList<T>(
        string title,
        IEnumerable<T> items,
        Func<T, string> displaySelector,
        ConsoleColor? itemColor = null
    )
    {
        var itemsList = items.ToList();
        if (!itemsList.Any())
            throw new InvalidOperationException($"No {title.ToLower()} available.");

        Console.Clear();
        Console.WriteLine($"\nAvailable {title}:");
        Console.WriteLine("═".PadRight(Console.WindowWidth - 1, '═'));

        // Create display options without extra newlines
        var options = itemsList.Select(item => displaySelector(item)).ToArray();

        var selectedIndex = ShowMenu(options, itemColor: itemColor);
        Console.WriteLine(); // Add a blank line after selection

        return itemsList[selectedIndex];
    }

    public string GetUserInput(string prompt, bool allowEmpty = false)
    {
        string? input;
        do
        {
            Console.Write($"{prompt}: ");
            input = Console.ReadLine()?.Trim();
        } while (!allowEmpty && string.IsNullOrWhiteSpace(input));

        return input ?? string.Empty;
    }

    public decimal GetDecimalInput(string prompt, decimal minValue = 0)
    {
        decimal value;
        do
        {
            Console.Write($"{prompt}: ");
            var input = Console.ReadLine();
            if (decimal.TryParse(input, out value) && value >= minValue)
                return value;

            Console.WriteLine($"Please enter a valid number greater than or equal to {minValue}");
        } while (true);
    }

    public DateTime GetDateInput(string prompt)
    {
        DateTime value;
        do
        {
            Console.Write($"{prompt} (yyyy-MM-dd): ");
            var input = Console.ReadLine();
            if (DateTime.TryParse(input, out value))
                return value;

            Console.WriteLine("Please enter a valid date in the format yyyy-MM-dd");
        } while (true);
    }
}
