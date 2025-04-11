#!/usr/bin/env bash

# -----------------
# BEGIN VARIABLES
# ----------------

AnalyzerVersion="1.1.3"
MonoGameBuildVersion="1.3.3.7-cpt"

WorkingDirectory="$(pwd)"
cd "$(dirname "${BASH_SOURCE[0]}")" || exit 1

ScriptRoot="$(pwd)"
SolutionDirectory="$(realpath "$ScriptRoot/..")"
AnalyzerDirectory="$(realpath "$SolutionDirectory/analyzers")"
NugetDirectory="$SolutionDirectory/.nuget"
MonoGameDirectory="$(realpath "$ScriptRoot/../libraries/MonoGame")"

# ---------------------
# BEGIN GUPPY ANALYZERS
# ---------------------

# Confirm .nuget directory exists
if [ ! -d "$NugetDirectory" ]; then
    mkdir -p "$NugetDirectory"
fi

# Build Analyzer package
dotnet build -c Release "$AnalyzerDirectory/Guppy.Analyzer.Core.Common"
dotnet pack -o "$NugetDirectory" /p:Version="$AnalyzerVersion" "$AnalyzerDirectory/Guppy.Analyzer.Core.Common"

# ---------------------------
# BEGIN MONOGAME INSTALLATION
# ---------------------------

cd "$MonoGameDirectory" || exit 1

if [ -f "./build.cake" ]; then
    dotnet tool restore
    dotnet cake --build-version "$MonoGameBuildVersion"

    MonoGameBuildVersion="${MonoGameBuildVersion}-develop"
else
    dotnet run --project "$MonoGameDirectory/build/Build.csproj" -- --build-version "$MonoGameBuildVersion"

    # Why isn't this included in Build.csproj tho?
    dotnet pack -o "$MonoGameDirectory/Artifacts/NuGet" /p:Version="$MonoGameBuildVersion" "$MonoGameDirectory/Tools/MonoGame.Content.Builder.Editor.Launcher/MonoGame.Content.Builder.Editor.Launcher.Linux.csproj"
fi

git reset --hard HEAD

cd "$WorkingDirectory" || exit 1

# Uninstall old tools (if any)
dotnet tool uninstall dotnet-mgcb || true
dotnet tool uninstall dotnet-mgcb-editor-linux || true
dotnet tool uninstall dotnet-mgcb-editor || true

dotnet tool install --version "$MonoGameBuildVersion" --add-source "$MonoGameDirectory/artifacts/NuGet" dotnet-mgcb
dotnet tool install --version "$MonoGameBuildVersion" --add-source "$MonoGameDirectory/artifacts/NuGet" dotnet-mgcb-editor-linux
dotnet tool install --version "$MonoGameBuildVersion" --add-source "$MonoGameDirectory/artifacts/NuGet" dotnet-mgcb-editor

# ---------------------
# BEGIN FILE GENERATION
# ---------------------

cat <<EOF > "$SolutionDirectory/Guppy.Core.Common.g.targets"
<!-- Generated via Guppy/scripts/install.sh -->
<Project>

  <PropertyGroup>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>

  <ItemGroup>
    <!-- Guppy packages are added to a local nuget feed via the 'Guppy/nuget.config' file -->
    <PackageReference Include="Guppy.Analyzer.Core.Common" Version="$AnalyzerVersion" />
  </ItemGroup>

</Project>
EOF

cat <<EOF > "$SolutionDirectory/nuget.config"
<!-- Generated via Guppy/scripts/install.sh -->
<configuration>
  <packageSources>
    <add key="GuppyPackages" value="$NugetDirectory" />
  </packageSources>
</configuration>
EOF