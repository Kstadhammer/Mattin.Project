using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mattin.Project.Infrastructure.Contexts;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Get the current assembly location
        var assemblyLocation = typeof(DesignTimeDbContextFactory).Assembly.Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;

        // Navigate up to solution root (3 levels up from bin/Debug/net9.0)
        var solutionDir = Path.GetFullPath(Path.Combine(assemblyDirectory, "../../.."));
        var dataDir = Path.Combine(solutionDir, "Data");

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
