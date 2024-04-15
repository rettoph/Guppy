using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Common.Attributes;
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
