using Mattin.Project.Core.Common;
using Mattin.Project.Core.Interfaces.Base;
using Mattin.Project.Core.Models.Entities;

namespace Mattin.Project.Core.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<Result<string>> GenerateProjectNumberAsync();
    Task<Result<IEnumerable<ProjectEntity>>> GetProjectsByClientIdAsync(int clientId);
}
