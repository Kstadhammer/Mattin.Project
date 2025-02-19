using System.ComponentModel.DataAnnotations;

namespace Mattin.Project.Core.Models.Entities;

public class ProjectManager : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(50)]
    public string? Department { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
