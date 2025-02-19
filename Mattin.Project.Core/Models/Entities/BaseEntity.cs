// Base entity implementation enhanced with AI assistance for:
// - Common entity properties
// - Audit trail functionality
// - Timestamp management
// - Entity tracking

namespace Mattin.Project.Core.Models.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
