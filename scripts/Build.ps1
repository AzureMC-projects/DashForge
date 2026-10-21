$ErrorActionPreference = 'Stop'
dotnet restore .\DashForge.sln
dotnet publish .\src\DashForge.App\DashForge.App.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o .\publish\app
if (-not (Test-Path .\publish\app\DashForge.exe)) { throw 'Publish output is missing DashForge.exe' }
