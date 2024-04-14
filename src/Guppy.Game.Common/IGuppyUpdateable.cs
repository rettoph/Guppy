using Guppy.Engine.Common;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public interface IGuppyUpdateable : ISequenceable<UpdateSequence>
    {
        void Update(GameTime gameTime);
    }
}
