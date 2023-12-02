using Guppy.Common;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public interface IGuppyDrawable : ISequenceable<DrawSequence>
    {
        void Draw(GameTime gameTime);
    }
}
