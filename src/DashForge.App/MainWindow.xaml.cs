using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;
namespace DashForge.App;
public partial class MainWindow : Window
{
    private string? _executable;
    public MainWindow() => InitializeComponent();
    private void SelectGame_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog { Title = "Select GeometryDash.exe", Filter = "Geometry Dash executable|GeometryDash.exe|Executables|*.exe", CheckFileExists = true };
        if (dialog.ShowDialog(this) != true) return;
        if (!string.Equals(Path.GetFileName(dialog.FileName), "GeometryDash.exe", StringComparison.OrdinalIgnoreCase)) { MessageBox.Show(this, "Choose GeometryDash.exe from a legitimate game installation.", "Unexpected executable", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
        _executable = Path.GetFullPath(dialog.FileName); var version = FileVersionInfo.GetVersionInfo(_executable).FileVersion;
        InstallationStatus.Text = $"Selected: {_executable}\nFile metadata: {version ?? "unavailable"}. This does not verify game build 2.2091."; PlayButton.IsEnabled = true; StatusText.Text = "Executable selected; compatibility remains unverified.";
    }
    private void Play_Click(object sender, RoutedEventArgs e)
    {
        if (_executable is null || !File.Exists(_executable)) { PlayButton.IsEnabled = false; MessageBox.Show(this, "Select a valid executable first."); return; }
        try { Process.Start(new ProcessStartInfo { FileName = _executable, WorkingDirectory = Path.GetDirectoryName(_executable)!, UseShellExecute = false }); StatusText.Text = $"Launch requested at {DateTimeOffset.Now:t}."; }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException or System.ComponentModel.Win32Exception) { MessageBox.Show(this, ex.Message, "Launch failed", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private void Nav(string title, string note) { PageTitle.Text = title; StatusText.Text = note; }
    private void Home_Click(object s, RoutedEventArgs e) => Nav("Your next attempt starts here.", "Home dashboard.");
    private void Installations_Click(object s, RoutedEventArgs e) => Nav("Installations", "Use SELECT GAME to choose an executable. Steam library discovery is not yet implemented.");
    private void Profiles_Click(object s, RoutedEventArgs e) => Nav("Profiles", "Profile model and JSON storage service are implemented; profile editor UI is not yet available.");
    private void Mods_Click(object s, RoutedEventArgs e) => Nav("Mod management", "Unavailable: no mod framework has been verified for this game build.");
    private void Backups_Click(object s, RoutedEventArgs e) => Nav("Backups", "Backup and restore UI is not available yet; no files have been modified.");
    private void Performance_Click(object s, RoutedEventArgs e) => Nav("Performance", "No unverified FPS, VSync, or graphics tweaks are applied.");
    private void Diagnostics_Click(object s, RoutedEventArgs e) => Nav("Diagnostics", "Basic in-session status only; structured log export is not yet implemented.");
    private void News_Click(object s, RoutedEventArgs e) => Nav("News & changelog", "Offline project status; no live news feed is configured.");
    private void Settings_Click(object s, RoutedEventArgs e) => Nav("Settings", "Persisted theme preferences and portable-mode UI are not yet wired up.");
}
