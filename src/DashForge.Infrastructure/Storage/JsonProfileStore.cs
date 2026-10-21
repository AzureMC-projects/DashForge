using System.Text.Json;
using DashForge.Core.Interfaces;
using DashForge.Core.Models;
namespace DashForge.Infrastructure.Storage;
public sealed class JsonProfileStore(string dataDirectory) : IProfileStore
{
    private readonly string _path = Path.Combine(dataDirectory, "profiles.json");
    private readonly SemaphoreSlim _gate = new(1, 1);
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    public async Task<IReadOnlyList<LauncherProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try {
            if (!File.Exists(_path)) { var initial = new[] { new LauncherProfile() }; await WriteAsync(initial, cancellationToken); return initial; }
            await using var stream = File.OpenRead(_path);
            var profiles = await JsonSerializer.DeserializeAsync<List<LauncherProfile>>(stream, Options, cancellationToken) ?? new();
            return profiles.Select(p => p.Validate()).ToArray();
        } catch (JsonException ex) { throw new InvalidDataException("Profile data is invalid JSON; preserve and inspect it before recovery.", ex); }
        finally { _gate.Release(); }
    }
    public async Task SaveAsync(LauncherProfile profile, CancellationToken cancellationToken = default)
    {
        profile.Validate(); await _gate.WaitAsync(cancellationToken);
        try { Directory.CreateDirectory(Path.GetDirectoryName(_path)!); var all = File.Exists(_path) ? (JsonSerializer.Deserialize<List<LauncherProfile>>(await File.ReadAllTextAsync(_path, cancellationToken), Options) ?? new()) : new(); var i = all.FindIndex(p => p.Id == profile.Id); if (i >= 0) all[i] = profile; else all.Add(profile); await WriteAsync(all, cancellationToken); }
        finally { _gate.Release(); }
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try { if (!File.Exists(_path)) return; var all = JsonSerializer.Deserialize<List<LauncherProfile>>(await File.ReadAllTextAsync(_path, cancellationToken), Options) ?? new(); all.RemoveAll(p => p.Id == id); if (all.Count == 0) all.Add(new LauncherProfile()); await WriteAsync(all, cancellationToken); }
        finally { _gate.Release(); }
    }
    private async Task WriteAsync(IReadOnlyList<LauncherProfile> profiles, CancellationToken token)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!); var temp = _path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try { await using (var stream = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous)) await JsonSerializer.SerializeAsync(stream, profiles, Options, token); File.Move(temp, _path, true); }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }
}
