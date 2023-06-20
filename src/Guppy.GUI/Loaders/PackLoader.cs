using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;
using Guppy.Resources;
using Guppy.Attributes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.GUI.Loaders
{
    [AutoLoad]
    internal sealed class PackLoader : IPackLoader
    {
        private readonly ContentManager _content;

        public PackLoader(ContentManager content)
        {
            _content = content;
        }

        public void Load(IResourcePackProvider packs)
        {
            _content.RootDirectory = GuppyPack.Path;

            packs.Configure(GuppyPack.Id, pack =>
            {
                pack.Add(Resources.Fonts.Default, _content.Load<SpriteFont>("Fonts/Default"));
            });
        }
    }
}
