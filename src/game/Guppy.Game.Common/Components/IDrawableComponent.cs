using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common.Components
{
    public interface IDrawableComponent
    {
        [RequireSequenceGroup<DrawComponentSequenceGroup>()]
        void Draw(GameTime gameTime);
    }
}
