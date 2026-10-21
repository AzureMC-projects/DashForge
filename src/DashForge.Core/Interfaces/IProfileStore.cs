using DashForge.Core.Models;
namespace DashForge.Core.Interfaces;
public interface IProfileStore
{
    Task<IReadOnlyList<LauncherProfile>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(LauncherProfile profile, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
