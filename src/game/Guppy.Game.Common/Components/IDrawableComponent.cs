using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common.Components
{
    public interface IDrawableComponent
    {
        [RequireSequenceGroup<DrawComponentSequenceGroupEnum>()]
        void Draw(GameTime gameTime);
    }
}