// Project status implementation enhanced with AI assistance for:
// - Status constants and validation
// - Business rules enforcement
// - Type safety
// - Domain-specific naming

namespace Mattin.Project.Core.Models.Entities;

public static class ProjectStatus
{
    public const string NotStarted = "Ej påbörjat";
    public const string InProgress = "Pågående";
    public const string Completed = "Avslutat";

    public static bool IsValid(string status) =>
        status switch
        {
            NotStarted or InProgress or Completed => true,
            _ => false,
        };
}
