using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Game.Graphics.Common.Resources;
using Guppy.Game.Graphics.NotImplemented.Resources;

namespace Guppy.Game.MonoGame.ResourceTypes
{
    internal class NotImplementedEffectCodeResourceType() : SimpleResourceType<IEffectCode>
    {
        public override string Name => "EffectCode";

        protected override bool TryResolve(ResourceKey<IEffectCode> resource, DirectoryLocation root, string input, out IEffectCode value)
        {
            value = new NotImplementedEffectCode();
            return true;
        }
    }
}
