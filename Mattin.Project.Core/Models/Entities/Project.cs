namespace Mattin.Project.Core.Models.Entities;

public static class ProjectStatus
{
    public const string NotStarted = "Ej påbörjat";
    public const string InProgress = "Pågående";
    public const string Completed = "Avslutat";

    public static bool IsValid(string status)
    {
        return status is NotStarted or InProgress or Completed;
    }
}

public class ProjectEntity : BaseEntity
{
    public string ProjectNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal TotalPrice { get; set; }

    // Relationships
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int StatusId { get; set; }
    public Status Status { get; set; } = null!;

    public int ProjectManagerId { get; set; }
    public ProjectManager ProjectManager { get; set; } = null!;
}
