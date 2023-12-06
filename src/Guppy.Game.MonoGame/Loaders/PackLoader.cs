using Guppy.Attributes;
using Guppy.Files.Enums;
using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.MonoGame.Loaders
{
    [AutoLoad]
    internal sealed class PackLoader : IPackLoader
    {
        public void Load(IResourcePackProvider packs)
        {
            packs.Register(FileType.CurrentDirectory, Path.Combine(GuppyMonoGamePack.Directory, "pack.json"));
        }
    }
}
