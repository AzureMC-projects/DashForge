# DashForge

A Windows desktop launcher for Geometry Dash, built with WPF and .NET 8. DashForge does not distribute game files and does not claim unverified compatibility with any game build.

## Status

Initial repository scaffold. The application focuses on safe installation selection, local profiles, launch configuration, logging, and build/release automation. Mod integration, verified repair manifests, and self-updating are intentionally unavailable until trusted interfaces and compatibility data are established.

## Requirements

- Windows 10/11 x64 for the WPF app
- .NET 8 SDK
- Inno Setup 6 to compile the installer

## Build and test

```powershell
./scripts/Test.ps1
./scripts/Build.ps1
```

## Portable mode

Create a `portable` directory beside the executable to store app data there. Otherwise settings live under `%LocalAppData%/DashForge`.

## Release

Push a `vX.Y.Z` tag. The Windows release workflow publishes a self-contained app, portable ZIP, installer, SHA-256 checksums, and release metadata. Configure repository Actions permissions to allow release creation. No signing certificate is assumed.

## Safety and limitations

- Geometry Dash must be acquired through a legitimate storefront; DashForge never downloads or redistributes it.
- Version detection is conservative and may report `Unknown` if trustworthy build evidence is unavailable.
- Launch arguments are passed as an argument list, not through a shell.
- Profiles store launcher settings, not isolated game saves.
- No mod framework is advertised as compatible unless a verified adapter is implemented.

See `CHANGELOG.md` for project history.