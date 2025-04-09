using Guppy.Core.Files.Common;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.AssetTypes;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.AssetTypes
{
    internal class NotImplementedSpriteFontAssetType() : SimpleAssetType<SpriteFont>
    {

        protected override bool TryResolve(AssetKey<SpriteFont> resource, DirectoryPath root, string input, out SpriteFont value)
        {
            value = default!;
            return true;
        }
    }
}