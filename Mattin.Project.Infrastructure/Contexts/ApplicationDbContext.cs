// Database context configuration enhanced with AI assistance for:
// - Entity relationships and constraints
// - Data seeding
// - Automatic timestamps
// - SQLite integration

using Mattin.Project.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Mattin.Project.Infrastructure.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<ProjectEntity> Projects { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Status> Statuses { get; set; } = null!;
    public DbSet<ProjectManager> ProjectManagers { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Status entity
        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.SortOrder).HasDefaultValue(0);
        });

        // Configure Project entity
        modelBuilder.Entity<ProjectEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProjectNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.HourlyRate).HasPrecision(10, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(15, 2);

            entity
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(p => p.Status)
                .WithMany(s => s.Projects)
                .HasForeignKey(p => p.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.ProjectManager)
                .WithMany(pm => pm.Projects)
                .HasForeignKey(p => p.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.Service)
                .WithMany(s => s.Projects)
                .HasForeignKey(p => p.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Client entity
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
        });

        // Configure ProjectManager entity
        modelBuilder.Entity<ProjectManager>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure Service entity
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.BasePrice).HasPrecision(10, 2);
            entity.Property(e => e.HourlyRate).HasPrecision(10, 2);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Add seed data
        var now = DateTime.UtcNow;

        // Seed ProjectManagers
        modelBuilder
            .Entity<ProjectManager>()
            .HasData(
                new ProjectManager
                {
                    Id = 1,
                    Name = "Anna Andersson",
                    Email = "anna.andersson@mattin-lassei.se",
                    PhoneNumber = "070-123 45 67",
                    Department = "Development",
                    IsActive = true,
                    Created = now,
                },
                new ProjectManager
                {
                    Id = 2,
                    Name = "Erik Eriksson",
                    Email = "erik.eriksson@mattin-lassei.se",
                    PhoneNumber = "070-234 56 78",
                    Department = "Design",
                    IsActive = true,
                    Created = now,
                },
                new ProjectManager
                {
                    Id = 3,
                    Name = "Maria Nilsson",
                    Email = "maria.nilsson@mattin-lassei.se",
                    PhoneNumber = "070-345 67 89",
                    Department = "Mobile Development",
                    IsActive = true,
                    Created = now,
                }
            );

        // Seed Statuses
        modelBuilder
            .Entity<Status>()
            .HasData(
                new Status
                {
                    Id = 1,
                    Name = "Not Started",
                    Description = "Project has not been started yet",
                    IsDefault = true,
                    SortOrder = 1,
                    Created = now,
                },
                new Status
                {
                    Id = 2,
                    Name = "In Progress",
                    Description = "Project is under development",
                    IsDefault = false,
                    SortOrder = 2,
                    Created = now,
                },
                new Status
                {
                    Id = 3,
                    Name = "Completed",
                    Description = "Project is finished",
                    IsDefault = false,
                    SortOrder = 3,
                    Created = now,
                }
            );

        // Seed Clients
        modelBuilder
            .Entity<Client>()
            .HasData(
                new Client
                {
                    Id = 1,
                    Name = "Mattin-Lassei Group AB",
                    Email = "info@mattin-lassei.se",
                    PhoneNumber = "08-123 45 67",
                    Address = "Kungsgatan 1, 111 43 Stockholm",
                    Created = now,
                },
                new Client
                {
                    Id = 2,
                    Name = "Tech Innovators AB",
                    Email = "contact@techinnovators.se",
                    PhoneNumber = "08-987 65 43",
                    Address = "Sveavägen 10, 111 57 Stockholm",
                    Created = now,
                }
            );

        // Seed Projects
        modelBuilder
            .Entity<ProjectEntity>()
            .HasData(
                new ProjectEntity
                {
                    Id = 1,
                    ProjectNumber = "2024-001",
                    Title = "Webbplats Redesign",
                    Description =
                        "Komplett redesign av företagets webbplats med fokus på användarupplevelse",
                    StatusId = 2, // Pågående
                    StartDate = now.AddDays(-30),
                    EndDate = now.AddDays(60),
                    ProjectManagerId = 1, // Anna Andersson
                    HourlyRate = 1200,
                    TotalPrice = 480000, // 400 timmar
                    ClientId = 1,
                    ServiceId = 1, // Web Development
                    Created = now,
                },
                new ProjectEntity
                {
                    Id = 2,
                    ProjectNumber = "2024-002",
                    Title = "E-handelsplattform",
                    Description =
                        "Utveckling av ny e-handelsplattform med integration mot befintliga system",
                    StatusId = 1, // Ej påbörjat
                    StartDate = now.AddDays(15),
                    EndDate = now.AddDays(105),
                    ProjectManagerId = 2, // Erik Eriksson
                    HourlyRate = 1100,
                    TotalPrice = 880000, // 800 timmar
                    ClientId = 2,
                    ServiceId = 1, // Web Development
                    Created = now,
                },
                new ProjectEntity
                {
                    Id = 3,
                    ProjectNumber = "2024-003",
                    Title = "App-utveckling",
                    Description = "Utveckling av mobilapp för intern kommunikation",
                    StatusId = 2, // Pågående
                    StartDate = now.AddDays(-15),
                    EndDate = now.AddDays(45),
                    ProjectManagerId = 3, // Maria Nilsson
                    HourlyRate = 1300,
                    TotalPrice = 416000, // 320 timmar
                    ClientId = 1,
                    ServiceId = 2, // Mobile App Development
                    Created = now,
                }
            );

        // Seed Services
        modelBuilder
            .Entity<Service>()
            .HasData(
                new Service
                {
                    Id = 1,
                    Name = "Web Development",
                    Description =
                        "Full-stack web development services including frontend and backend",
                    BasePrice = 50000,
                    HourlyRate = 1200,
                    Category = "Development",
                    IsActive = true,
                    Created = now,
                },
                new Service
                {
                    Id = 2,
                    Name = "Mobile App Development",
                    Description = "Native and cross-platform mobile application development",
                    BasePrice = 75000,
                    HourlyRate = 1300,
                    Category = "Development",
                    IsActive = true,
                    Created = now,
                },
                new Service
                {
                    Id = 3,
                    Name = "UI/UX Design",
                    Description = "User interface and experience design for digital products",
                    BasePrice = 35000,
                    HourlyRate = 1100,
                    Category = "Design",
                    IsActive = true,
                    Created = now,
                },
                new Service
                {
                    Id = 4,
                    Name = "Cloud Infrastructure",
                    Description =
                        "Setup and management of cloud infrastructure and DevOps practices",
                    BasePrice = 65000,
                    HourlyRate = 1400,
                    Category = "Infrastructure",
                    IsActive = true,
                    Created = now,
                },
                new Service
                {
                    Id = 5,
                    Name = "Security Audit",
                    Description = "Comprehensive security assessment and penetration testing",
                    BasePrice = 45000,
                    HourlyRate = 1500,
                    Category = "Security",
                    IsActive = true,
                    Created = now,
                },
                new Service
                {
                    Id = 6,
                    Name = "Data Analytics",
                    Description = "Business intelligence and data analytics solutions",
                    BasePrice = 55000,
                    HourlyRate = 1250,
                    Category = "Analytics",
                    IsActive = true,
                    Created = now,
                }
            );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.Modified = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
