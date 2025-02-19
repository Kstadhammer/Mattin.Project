namespace Mattin.Project.Core.Models.Entities;

public class Status : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsDefault { get; set; }
    public int SortOrder { get; set; }

    // Navigation properties
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
