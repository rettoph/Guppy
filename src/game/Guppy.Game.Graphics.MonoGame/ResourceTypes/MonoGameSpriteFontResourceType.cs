using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.ResourceTypes
{
    internal class MonoGameSpriteFontResourceType(IContentManager content) : SimpleResourceType<SpriteFont>
    {

        private readonly IContentManager _content = content;

        protected override bool TryResolve(Resource<SpriteFont> resource, DirectoryLocation root, string input, out SpriteFont value)
        {
            _content.Value.RootDirectory = root.Path;

            value = _content.Value.Load<SpriteFont>(DirectoryHelper.Normalize(input));

            return true;
        }
    }
}
