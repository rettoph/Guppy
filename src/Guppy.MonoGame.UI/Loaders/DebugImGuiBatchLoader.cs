using Guppy.Common;
using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.UI.Constants;
using Guppy.Resources.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.UI.Providers;

namespace Guppy.MonoGame.UI.Loaders
{
    internal sealed class DebugImGuiBatchLoader : IImGuiBatchLoader
    {
        public void Load(IImGuiBatchProvider batches)
        {
            batches.Get(ImGuiBatchConstants.Debug).Fonts.Add(ResourceConstants.DiagnosticsImGuiFont, ResourceConstants.DiagnosticsImGuiFontHeader);
        }
    }
}
