namespace Mattin.Project.Core.Models.Entities;

public class ProjectManager : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
