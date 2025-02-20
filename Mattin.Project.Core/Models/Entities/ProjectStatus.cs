// Project status implementation enhanced with AI assistance for:
// - Status constants and validation
// - Business rules enforcement
// - Type safety
// - Domain-specific naming

namespace Mattin.Project.Core.Models.Entities;

public static class ProjectStatus
{
    public const string NotStarted = "Not Started";
    public const string InProgress = "In Progress";
    public const string Completed = "Completed";

    public static bool IsValid(string status) =>
        status switch
        {
            NotStarted or InProgress or Completed => true,
            _ => false,
        };
}
