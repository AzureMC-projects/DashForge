$ErrorActionPreference = 'Stop'
dotnet restore .\DashForge.sln
dotnet test .\DashForge.sln -c Release --no-restore
