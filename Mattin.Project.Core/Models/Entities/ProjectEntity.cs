// Project entity model enhanced with AI assistance for:
// - Data validation and constraints
// - Relationship modeling
// - Business rules implementation
// - Domain model design

using System.ComponentModel.DataAnnotations;

namespace Mattin.Project.Core.Models.Entities;

public class ProjectEntity : BaseEntity
{
    [Required]
    [StringLength(20)]
    public string ProjectNumber { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal HourlyRate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }

    // Relationships
    [Required]
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    [Required]
    public int StatusId { get; set; }
    public Status Status { get; set; } = null!;

    [Required]
    public int ProjectManagerId { get; set; }
    public ProjectManager ProjectManager { get; set; } = null!;

    public int? ServiceId { get; set; }
    public Service? Service { get; set; }
}
