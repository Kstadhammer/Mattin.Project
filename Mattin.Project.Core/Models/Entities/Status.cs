using System.ComponentModel.DataAnnotations;

namespace Mattin.Project.Core.Models.Entities;

public class Status : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = null!;

    [Required]
    public bool IsDefault { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int SortOrder { get; set; }

    // Navigation properties
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
