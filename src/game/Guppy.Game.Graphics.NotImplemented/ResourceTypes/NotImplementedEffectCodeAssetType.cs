using Guppy.Core.Files.Common;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.AssetTypes;
using Guppy.Game.Graphics.Common.Assets;
using Guppy.Game.Graphics.NotImplemented.Assets;

namespace Guppy.Game.MonoGame.AssetTypes
{
    internal class NotImplementedEffectCodeAssetType() : SimpleAssetType<IEffectCode>
    {
        public override string Name => "EffectCode";

        protected override bool TryResolve(AssetKey<IEffectCode> resource, DirectoryPath root, string input, out IEffectCode value)
        {
            value = new NotImplementedEffectCode();
            return true;
        }
    }
}