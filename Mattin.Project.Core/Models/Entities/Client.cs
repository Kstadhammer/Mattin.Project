namespace Mattin.Project.Core.Models.Entities;

public class Client : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }

    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
