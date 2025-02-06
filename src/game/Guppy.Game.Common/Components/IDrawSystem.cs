using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common.Systems
{
    public interface IDrawSystem
    {
        [RequireSequenceGroup<DrawSequenceGroupEnum>()]
        void Draw(GameTime gameTime);
    }
}