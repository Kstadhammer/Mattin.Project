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
}
