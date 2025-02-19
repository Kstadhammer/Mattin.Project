namespace Mattin.Project.Core.Models.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
