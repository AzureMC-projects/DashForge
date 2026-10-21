using DashForge.Core.Models;
using DashForge.Infrastructure.Launching;
using DashForge.Infrastructure.Storage;
namespace DashForge.Core.Tests;
public sealed class ProfileAndLaunchTests
{
    [Fact] public void Profile_TrimsName_AndRejectsUnknownPreset() { Assert.Equal("My profile", new LauncherProfile { Name = " My profile " }.Validate().Name); Assert.Throws<ArgumentException>(() => new LauncherProfile { PerformancePreset = "Turbo" }.Validate()); }
    [Fact] public void ArgumentParser_PreservesQuotedArgument() { Assert.Equal(new[] { "--level", "my level" }, GameLauncher.ParseArguments("--level \"my level\"")); }
    [Fact] public void ArgumentParser_RejectsUnmatchedQuote() => Assert.Throws<ArgumentException>(() => GameLauncher.ParseArguments("\"broken"));
    [Fact] public async Task ProfileStore_PersistsAndDeletes() { var dir = Path.Combine(Path.GetTempPath(), "DashForgeTests", Guid.NewGuid().ToString("N")); try { var store = new JsonProfileStore(dir); var profile = new LauncherProfile { Name = "Test" }; await store.SaveAsync(profile); Assert.Contains(await store.GetAllAsync(), p => p.Id == profile.Id); await store.DeleteAsync(profile.Id); Assert.Single(await store.GetAllAsync()); } finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); } }
}
