using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common.Components
{
    public interface IDebugComponent
    {
        [RequireSequenceGroup<DebugSequenceGroupEnum>]
        void DrawDebug(GameTime gameTime);
    }
}