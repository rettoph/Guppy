using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;
using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.Resources;
using Guppy.GUI.Constants;
using Guppy.Attributes;

namespace Guppy.GUI.Loaders
{
    [AutoLoad]
    internal sealed class PackLoader : IPackLoader
    {
        public void Load(IPackProvider packs)
        {
            packs.GetById(GuppyPack.Id).Add(new IResource[]
            {
                new SpriteFontResource(Fonts.Default, "Fonts/Default")
            });
        }
    }
}
