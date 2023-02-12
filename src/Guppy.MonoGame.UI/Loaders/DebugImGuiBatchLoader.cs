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
using Guppy.Attributes;

namespace Guppy.MonoGame.UI.Loaders
{
    internal sealed class DebugImGuiBatchLoader : IGuppyLoader
    {
        private readonly IImGuiBatchProvider _batches;

        public DebugImGuiBatchLoader(IImGuiBatchProvider batchs)
        {
            _batches = batchs;
        }

        public void Load(IGuppy guppy)
        {
            var batch = _batches.Get(ImGuiBatchConstants.Default);

            batch.Fonts.Add(ResourceConstants.DiagnosticsImGuiFont, ResourceConstants.DiagnosticsImGuiFontHeader);
            batch.RebuildFontAtlas();
        }
    }
}
