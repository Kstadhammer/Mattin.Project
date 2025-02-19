namespace Mattin.Project.Presentation.Helpers;

public class MenuHelper
{
    public int ShowMenu(string[] options, int selectedIndex = 0)
    {
        ConsoleKey keyPressed;
        do
        {
            Console.CursorVisible = false;
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
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
                Console.CursorTop -= options.Length;
            }
            else if (keyPressed == ConsoleKey.DownArrow)
            {
                selectedIndex++;
                if (selectedIndex >= options.Length)
                    selectedIndex = 0;
                Console.CursorTop -= options.Length;
            }
        } while (keyPressed != ConsoleKey.Enter);

        Console.CursorVisible = true;
        return selectedIndex;
    }

    public T SelectFromList<T>(string title, IEnumerable<T> items, Func<T, string> displaySelector)
    {
        var itemsList = items.ToList();
        if (!itemsList.Any())
            throw new InvalidOperationException($"No {title.ToLower()} available.");

        Console.WriteLine($"\nAvailable {title}:");
        Console.WriteLine("═".PadRight(Console.WindowWidth - 1, '═'));

        // Create display options without extra newlines
        var options = itemsList.Select(item => displaySelector(item)).ToArray();

        var selectedIndex = ShowMenu(options);
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
