using System.ComponentModel.DataAnnotations;
using Mattin.Project.Core.Models.DTOs.Base;

namespace Mattin.Project.Core.Models.DTOs.Project;

public class UpdateProjectDto : BaseDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public int ProjectManagerId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal HourlyRate { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }

    [Required]
    public int ClientId { get; set; }
}
