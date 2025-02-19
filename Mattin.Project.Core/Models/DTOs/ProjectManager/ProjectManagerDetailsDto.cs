using Mattin.Project.Core.Models.DTOs.Base;
using Mattin.Project.Core.Models.DTOs.Project;

namespace Mattin.Project.Core.Models.DTOs.ProjectManager;

public class ProjectManagerDetailsDto : BaseDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Department { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public ICollection<ProjectDetailsDto> Projects { get; set; } = new List<ProjectDetailsDto>();

    // Calculated properties
    public int ActiveProjectsCount => Projects.Count(p => !p.IsCompleted);
    public int CompletedProjectsCount => Projects.Count(p => p.IsCompleted);
    public decimal TotalProjectValue => Projects.Sum(p => p.TotalPrice);
    public string FormattedTotalProjectValue => $"{TotalProjectValue:C}";
}
