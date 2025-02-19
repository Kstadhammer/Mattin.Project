using Mattin.Project.Core.Models.DTOs.Base;
using Mattin.Project.Core.Models.DTOs.Project;

namespace Mattin.Project.Core.Models.DTOs.Service;

public class ServiceDetailsDto : BaseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsActive { get; set; }
    public string? Category { get; set; }

    // Navigation properties
    public ICollection<ProjectDetailsDto> Projects { get; set; } = new List<ProjectDetailsDto>();

    // Calculated properties
    public int ActiveProjectsCount => Projects.Count(p => !p.IsCompleted);
    public string FormattedBasePrice => $"{BasePrice:C}";
    public string FormattedHourlyRate => $"{HourlyRate:C}/tim";
}
