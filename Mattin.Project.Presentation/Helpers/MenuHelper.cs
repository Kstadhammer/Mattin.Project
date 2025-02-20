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
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ResetColor();
                    if (itemColor.HasValue)
                    {
                        Console.ForegroundColor = itemColor.Value;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
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
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("═".PadRight(Console.WindowWidth - 1, '═'));
        Console.ResetColor();

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
        decimal value = minValue;
        decimal increment = 1;
        ConsoleKey keyPressed;

        do
        {
            Console.CursorVisible = false;
            Console.Write($"\r{prompt}: {value:N2}   ");

            var keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    value = Math.Max(minValue, value + increment);
                    break;
                case ConsoleKey.DownArrow:
                    value = Math.Max(minValue, value - increment);
                    break;
                case ConsoleKey.RightArrow:
                    increment *= 10;
                    Console.Write($" (Step: {increment:N0})   ");
                    break;
                case ConsoleKey.LeftArrow:
                    increment = Math.Max(0.01M, increment / 10);
                    Console.Write($" (Step: {increment:N2})   ");
                    break;
                case ConsoleKey.D:
                    if (keyInfo.Modifiers == ConsoleModifiers.Control)
                    {
                        string? input = null;
                        Console.Write($"\r{prompt}: ");
                        Console.CursorVisible = true;
                        input = Console.ReadLine()?.Trim();
                        if (
                            decimal.TryParse(input, out decimal parsedValue)
                            && parsedValue >= minValue
                        )
                        {
                            value = parsedValue;
                            keyPressed = ConsoleKey.Enter;
                        }
                        Console.CursorVisible = false;
                    }
                    break;
            }
        } while (keyPressed != ConsoleKey.Enter);

        Console.WriteLine(); // Move to next line
        Console.CursorVisible = true;
        return value;
    }

    public DateTime GetDateInput(string prompt)
    {
        DateTime value = DateTime.Today;
        ConsoleKey keyPressed;

        do
        {
            Console.CursorVisible = false;
            Console.Write($"\r{prompt} (yyyy-MM-dd): {value:yyyy-MM-dd}   ");

            var keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    value = value.AddDays(1);
                    break;
                case ConsoleKey.DownArrow:
                    value = value.AddDays(-1);
                    break;
                case ConsoleKey.LeftArrow:
                    value = value.AddMonths(-1);
                    break;
                case ConsoleKey.RightArrow:
                    value = value.AddMonths(1);
                    break;
                case ConsoleKey.PageUp:
                    value = value.AddYears(1);
                    break;
                case ConsoleKey.PageDown:
                    value = value.AddYears(-1);
                    break;
                case ConsoleKey.T:
                    value = DateTime.Today;
                    break;
                case ConsoleKey.D:
                    if (keyInfo.Modifiers == ConsoleModifiers.Control)
                    {
                        string? input = null;
                        Console.Write($"\r{prompt} (yyyy-MM-dd): ");
                        Console.CursorVisible = true;
                        input = Console.ReadLine()?.Trim();
                        if (DateTime.TryParse(input, out DateTime parsedDate))
                        {
                            value = parsedDate;
                            keyPressed = ConsoleKey.Enter;
                        }
                        Console.CursorVisible = false;
                    }
                    break;
            }
        } while (keyPressed != ConsoleKey.Enter);

        Console.WriteLine(); // Move to next line
        Console.CursorVisible = true;
        return value;
    }
}
