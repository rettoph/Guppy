using Guppy.Attributes;
using Guppy.Files.Enums;
using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;

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
