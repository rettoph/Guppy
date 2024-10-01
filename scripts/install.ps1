# ---------------
# BEGIN VARIABLES
# ---------------

$AnalyzerVersion = "1.0.9"
$MonoGameBuildVersion = "1.3.3.7-cpt";


$WorkingDirectory = Get-Location;
Set-Location $PSScriptRoot;

$SolutionDirectory = "$($PSScriptRoot)/.." | Resolve-Path
$AnalyzerDirectory = "$($SolutionDirectory)/analyzers" | Resolve-Path
$NugetDirectory = "$($SolutionDirectory)/.nuget"
$MonoGameDirectory = "$($PSScriptRoot)/../libraries/MonoGame" | Resolve-Path;

# ---------------------
# BEGIN GUPPY ANALYZERS
# ---------------------

# Confirm .nuget directory exists
If(!(test-path -PathType container $NugetDirectory))
{
      New-Item -ItemType Directory -Path $NugetDirectory
}

$NugetDirectory = $NugetDirectory | Resolve-Path

# Build Analyzer package
dotnet build -c Release "$($AnalyzerDirectory)/Guppy.Analyzer.Core.Common"
dotnet pack -o $NugetDirectory /p:Version=$AnalyzerVersion "$($AnalyzerDirectory)/Guppy.Analyzer.Core.Common"


# ---------------------------
# BEGIN MONOGAME INSTALLATION
# ---------------------------

# Build MonoGame
Set-Location $MonoGameDirectory;

if((Test-Path "./build.cake") -eq $true)
{
    dotnet tool restore
    dotnet cake --build-version $MonoGameBuildVersion

    $MonoGameBuildVersion = "$($MonoGameBuildVersion)-develop"
}
else {
    dotnet run --project "$($MonoGameDirectory)/build/Build.csproj" -- "--build-version" $MonoGameBuildVersion

    # Why isn't this included in Build.csproj tho?
    dotnet pack -o "$($MonoGameDirectory)/Artifacts/NuGet" /p:Version=$MonoGameBuildVersion "$($MonoGameDirectory)/Tools/MonoGame.Content.Builder.Editor.Launcher/MonoGame.Content.Builder.Editor.Launcher.Windows.csproj"
}

git reset --hard HEAD

Set-Location $WorkingDirectory;

# Uninstall old tools (if any)
dotnet tool uninstall dotnet-mgcb
dotnet tool uninstall dotnet-mgcb-editor-windows
dotnet tool uninstall dotnet-mgcb-editor

# https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create
# https://learn.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use
dotnet tool install --version $MonoGameBuildVersion --add-source "$($MonoGameDirectory)/Artifacts/NuGet" dotnet-mgcb
dotnet tool install --version $MonoGameBuildVersion --add-source "$($MonoGameDirectory)/Artifacts/NuGet" dotnet-mgcb-editor-windows
dotnet tool install --version $MonoGameBuildVersion --add-source "$($MonoGameDirectory)/Artifacts/NuGet" dotnet-mgcb-editor

# ---------------------
# BEGIN FILE GENERATION
# ---------------------

@'
<!-- Generated via Guppy/scripts/install.ps1 -->
<Project>

  <PropertyGroup>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>

  <ItemGroup>
    <!-- Guppy packages are added to a local nuget feed via the 'Guppy/nuget.config' file -->
    <PackageReference Include="Guppy.Analyzer.Core.Common" Version="{0}" />
  </ItemGroup>

</Project>
'@ -f $AnalyzerVersion | Out-File -FilePath "$($SolutionDirectory)/Guppy.Core.Common.g.targets"

@'
<!-- Generated via Guppy/scripts/install.ps1 -->
<configuration>
  <packageSources>
    <add key="GuppyPackages" value="{0}" />
  </packageSources>
</configuration>
'@ -f $NugetDirectory | Out-File -FilePath "$($SolutionDirectory)/nuget.config"