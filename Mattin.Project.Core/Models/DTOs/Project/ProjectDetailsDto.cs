using Mattin.Project.Core.Models.DTOs.Base;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Core.Models.DTOs.ProjectManager;

namespace Mattin.Project.Core.Models.DTOs.Project;

public class ProjectDetailsDto : BaseDto
{
    public string ProjectNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation properties
    public int ClientId { get; set; }
    public ClientDetailsDto Client { get; set; } = null!;

    public int ProjectManagerId { get; set; }
    public ProjectManagerDetailsDto ProjectManager { get; set; } = null!;

    // Calculated properties
    public int DurationInDays => (EndDate ?? DateTime.MaxValue).Subtract(StartDate).Days;
    public bool IsCompleted => Status == Entities.ProjectStatus.Completed;
    public string FormattedTotalPrice => $"{TotalPrice:C}";
    public string FormattedHourlyRate => $"{HourlyRate:C}/tim";
}
