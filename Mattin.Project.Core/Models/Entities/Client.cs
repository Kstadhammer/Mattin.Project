// Client entity model enhanced with AI assistance for:
// - Data validation attributes
// - Property constraints
// - Relationship definitions
// - Domain model design

using System.ComponentModel.DataAnnotations;

namespace Mattin.Project.Core.Models.Entities;

public class Client : BaseEntity
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

    [StringLength(200)]
    public string? Address { get; set; }

    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
