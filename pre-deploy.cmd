dotnet restore

dotnet build TauCode.Data.Graphs.sln -c Debug
dotnet build TauCode.Data.Graphs.sln -c Release

dotnet test TauCode.Data.Graphs.sln -c Debug
dotnet test TauCode.Data.Graphs.sln -c Release

nuget pack nuget\TauCode.Data.Graphs.nuspec