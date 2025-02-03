using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common.Systems
{
    public interface IDebugSystem
    {
        [RequireSequenceGroup<DebugSequenceGroupEnum>]
        void DrawDebug(GameTime gameTime);
    }
}