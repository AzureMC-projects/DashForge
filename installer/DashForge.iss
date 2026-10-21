#define MyAppName "DashForge"
#define MyAppVersion "0.1.0"
[Setup]
AppId={{6F8A1B91-7E17-45A2-9D6D-A2F8D5CBA110}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={localappdata}\Programs\DashForge
DefaultGroupName=DashForge
PrivilegesRequired=lowest
OutputDir=output
OutputBaseFilename=DashForge-Setup-{#MyAppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
[Files]
Source: "..\publish\app\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
[Icons]
Name: "{group}\DashForge"; Filename: "{app}\DashForge.exe"
Name: "{autodesktop}\DashForge"; Filename: "{app}\DashForge.exe"; Tasks: desktopicon
[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"
[Run]
Filename: "{app}\DashForge.exe"; Description: "Launch DashForge"; Flags: nowait postinstall skipifsilent
