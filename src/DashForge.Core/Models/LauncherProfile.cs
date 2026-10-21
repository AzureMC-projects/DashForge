namespace DashForge.Core.Models;

public sealed record LauncherProfile
{
    public int SchemaVersion { get; init; } = 1;
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = "Default";
    public string? Description { get; init; }
    public string? InstallationPath { get; init; }
    public string LaunchArguments { get; init; } = "";
    public string PerformancePreset { get; init; } = "Default";
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUsedAt { get; init; } = DateTimeOffset.UtcNow;
    public LauncherProfile Validate()
    {
        if (SchemaVersion != 1) throw new InvalidOperationException($"Unsupported profile schema: {SchemaVersion}.");
        if (string.IsNullOrWhiteSpace(Name) || Name.Trim().Length > 64) throw new ArgumentException("Profile name must contain 1–64 characters.", nameof(Name));
        if (LaunchArguments.Length > 2048) throw new ArgumentException("Launch arguments are too long.", nameof(LaunchArguments));
        if (PerformancePreset is not ("Default" or "Balanced" or "High-refresh" or "Custom")) throw new ArgumentException("Unknown performance preset.", nameof(PerformancePreset));
        return this with { Name = Name.Trim() };
    }
}
