using Guppy.MonoGame.UI.Constants;
using Guppy.MonoGame.UI.Resources;
using Guppy.Resources;
using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Loaders
{
    internal sealed class PackLoader : IPackLoader
    {
        public void Load(IPackProvider packs)
        {
            packs.GetById(GuppyPack.Id).Add(new IResource[]
            {
                new TrueTypeFontResource(ResourceConstants.DiagnosticsTTF, FileConstants.DiagnosticsTTF),
                new ImGuiFontResource(ResourceConstants.DiagnosticsImGuiFont, ResourceConstants.DiagnosticsTTF, 18),
                new ImGuiFontResource(ResourceConstants.DiagnosticsImGuiFontHeader, ResourceConstants.DiagnosticsTTF, 20),
            });
        }
    }
}
