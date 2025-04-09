using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.AssetTypes;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.AssetTypes
{
    internal class MonoGameSpriteFontAssetType(IContentManager content) : SimpleAssetType<SpriteFont>
    {

        private readonly IContentManager _content = content;

        protected override bool TryResolve(AssetKey<SpriteFont> resource, DirectoryPath root, string input, out SpriteFont value)
        {
            this._content.Value.RootDirectory = root.Path;

            value = this._content.Value.Load<SpriteFont>(DirectoryHelper.Normalize(input));

            return true;
        }
    }
}