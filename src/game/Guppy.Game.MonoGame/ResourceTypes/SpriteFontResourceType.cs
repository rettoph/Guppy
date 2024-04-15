using Guppy.Engine.Attributes;
using Guppy.Core.Files;
using Guppy.Core.Files.Helpers;
using Guppy.Core.Resources;
using Guppy.Core.Resources.ResourceTypes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.ResourceTypes
{
    [AutoLoad]
    internal class SpriteFontResourceType : SimpleResourceType<SpriteFont>
    {
        public Type Type => typeof(SpriteFont);

        private readonly ContentManager _content;

        public SpriteFontResourceType(ContentManager content)
        {
            _content = content;
        }

        protected override bool TryResolve(Resource<SpriteFont> resource, DirectoryLocation root, string input, out SpriteFont value)
        {
            _content.RootDirectory = root.Path;

            value = _content.Load<SpriteFont>(DirectoryHelper.Normalize(input));
            return true;
        }
    }
}
