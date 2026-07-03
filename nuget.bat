dotnet test .\StacyClouds.C4Sharp.Core.Tests\StacyClouds.C4Sharp.Core.Tests.csproj
dotnet test .\StacyClouds.C4Sharp.Client.Tests\StacyClouds.C4Sharp.Client.Tests.csproj

dotnet msbuild "/t:rebuild;pack" /p:Version=0.9.7 /p:Configuration=Debug .\StacyClouds.C4Sharp.Core\StacyClouds.C4Sharp.Core.csproj
dotnet msbuild "/t:rebuild;pack" /p:Version=0.9.7 /p:Configuration=Debug .\StacyClouds.C4Sharp.Client\StacyClouds.C4Sharp.Client.csproj