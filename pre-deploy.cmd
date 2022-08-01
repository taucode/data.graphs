dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.Data.Graphs.Tests\TauCode.Data.Graphs.Tests.csproj
dotnet test -c Release .\test\TauCode.Data.Graphs.Tests\TauCode.Data.Graphs.Tests.csproj

nuget pack nuget\TauCode.Data.Graphs.nuspec