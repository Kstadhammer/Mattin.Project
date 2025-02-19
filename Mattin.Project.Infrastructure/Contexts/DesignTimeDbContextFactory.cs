using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mattin.Project.Infrastructure.Contexts;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Get the solution root directory
        var currentDirectory = Directory.GetCurrentDirectory();
        var solutionDir = Path.GetFullPath(Path.Combine(currentDirectory, ".."));
        while (!File.Exists(Path.Combine(solutionDir, "Mattin.Project.sln")))
        {
            solutionDir = Path.GetFullPath(Path.Combine(solutionDir, ".."));
            if (solutionDir == Path.GetPathRoot(solutionDir))
            {
                throw new InvalidOperationException("Could not find solution root directory.");
            }
        }

        var dataDir = Path.Combine(solutionDir, "Data");
        Console.WriteLine($"Solution directory: {solutionDir}"); // Debug line

        // Create the directory if it doesn't exist
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir);
        }

        var dbPath = Path.Combine(dataDir, "mattinproject.db");
        Console.WriteLine($"Database path: {dbPath}"); // Debug line

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
