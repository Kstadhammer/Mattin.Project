namespace Mattin.Project.Presentation.Helpers;

public static class LogoHelper
{
    public static void DisplayLogo()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(
            @"
╔═══════════════════════════════════════════════════════════════════════════╗
║  __  __       _   _   _         _                        _                ║
║ |  \/  | __ _| |_| |_(_)_ __   | |    __ _ ___ ___  ___(_)              ║
║ | |\/| |/ _` | __| __| | '_ \  | |   / _` / __/ __|/ _ \ |              ║
║ | |  | | (_| | |_| |_| | | | | | |__| (_| \__ \__ \  __/ |              ║
║ |_|  |_|\__,_|\__|\__|_|_| |_| |_____\__,_|___/___/\___|_|              ║
║                                                                           ║
║                    Project Management System                              ║
╚═══════════════════════════════════════════════════════════════════════════╝"
        );
        Console.ResetColor();
    }

    public static void DisplaySmallLogo()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(
            @"
╔═══════════════════════════════════════════════════╗
║  __  __       _   _   _         _                ║
║ |  \/  | __ _| |_| |_(_)_ __   | |              ║
║ | |\/| |/ _` | __| __| | '_ \  | |              ║
║ |_|  |_|\__,_|\__|\__|_|_| |_| |_|              ║
║                                                   ║
║            Project Management System              ║
╚═══════════════════════════════════════════════════╝"
        );
        Console.ResetColor();
    }

    public static void DisplayMatrixEffect()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        var text =
            @"
   __  __       _   _   _         _                        _ 
  |  \/  | __ _| |_| |_(_)_ __   | |    __ _ ___ ___  ___(_)
  | |\/| |/ _` | __| __| | '_ \  | |   / _` / __/ __|/ _ \ |
  | |  | | (_| | |_| |_| | | | | | |__| (_| \__ \__ \  __/ |
  |_|  |_|\__,_|\__|\__|_|_| |_| |_____\__,_|___/___/\___|_|";

        var lines = text.Split('\n');
        var maxLength = lines.Max(l => l.Length);
        var chars = new List<(int row, int col, char ch, DateTime time)>();

        // Initialize characters with random timing
        var random = new Random();
        var currentTime = DateTime.Now;
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                if (lines[row][col] != ' ')
                {
                    chars.Add(
                        (
                            row + 3,
                            col + 5,
                            lines[row][col],
                            currentTime.AddMilliseconds(random.Next(0, 2000))
                        )
                    );
                }
            }
        }

        // Sort by time to reveal characters in sequence
        chars = chars.OrderBy(x => x.time).ToList();

        // Display matrix rain effect
        foreach (var (row, col, ch, time) in chars)
        {
            var delay = (time - currentTime).Milliseconds;
            if (delay > 0)
            {
                Thread.Sleep(Math.Min(delay, 50));
            }

            // Matrix rain effect
            for (int i = 0; i < row; i++)
            {
                Console.SetCursorPosition(col, i);
                Console.Write(random.Next(2) == 0 ? '1' : '0');
                Thread.Sleep(5);
            }

            Console.SetCursorPosition(col, row);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(ch);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(0, lines.Length + 5);
        Console.WriteLine("\n  Press Enter to return to main menu...");
        Console.CursorVisible = true;

        while (Console.ReadKey(true).Key != ConsoleKey.Enter)
        {
            // Wait for Enter key
        }

        Console.Clear();
        Console.ResetColor();
    }
}
