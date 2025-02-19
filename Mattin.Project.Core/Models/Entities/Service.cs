// Service entity model enhanced with AI assistance for:
// - Data validation attributes
// - Property constraints
// - Relationship definitions
// - Domain model design

using System.ComponentModel.DataAnnotations;

namespace Mattin.Project.Core.Models.Entities;

public class Service : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = null!;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal BasePrice { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal HourlyRate { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [StringLength(50)]
    public string? Category { get; set; }

    // Navigation properties
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
