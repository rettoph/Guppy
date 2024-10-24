using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.ResourceTypes
{
    internal class NotImplementedSpriteFontResourceType() : SimpleResourceType<SpriteFont>
    {

        protected override bool TryResolve(ResourceKey<SpriteFont> resource, DirectoryLocation root, string input, out SpriteFont value)
        {
            value = default!;
            return true;
        }
    }
}
