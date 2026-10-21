using System.Diagnostics;
namespace DashForge.Infrastructure.Launching;
public sealed class GameLauncher
{
    public Process Start(string executablePath, IEnumerable<string> arguments)
    {
        if (string.IsNullOrWhiteSpace(executablePath) || !File.Exists(executablePath)) throw new FileNotFoundException("Select a valid Geometry Dash executable before launching.", executablePath);
        var fullPath = Path.GetFullPath(executablePath);
        var info = new ProcessStartInfo { FileName = fullPath, WorkingDirectory = Path.GetDirectoryName(fullPath)!, UseShellExecute = false };
        foreach (var arg in arguments) info.ArgumentList.Add(arg);
        return Process.Start(info) ?? throw new InvalidOperationException("Windows did not start the game process.");
    }
    public static IReadOnlyList<string> ParseArguments(string input)
    {
        if (input.Length > 2048) throw new ArgumentException("Arguments are limited to 2048 characters.", nameof(input));
        var result = new List<string>(); var current = new System.Text.StringBuilder(); var quoted = false;
        foreach (var c in input) { if (c == '"') quoted = !quoted; else if (char.IsWhiteSpace(c) && !quoted) { if (current.Length > 0) { result.Add(current.ToString()); current.Clear(); } } else current.Append(c); }
        if (quoted) throw new ArgumentException("Launch arguments contain an unmatched quote.", nameof(input));
        if (current.Length > 0) result.Add(current.ToString()); return result;
    }
}
