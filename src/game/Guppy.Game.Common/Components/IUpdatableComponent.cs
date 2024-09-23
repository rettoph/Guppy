using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common.Components
{
    public interface IUpdatableComponent
    {
        [RequireSequence<UpdateComponentSequence>()]
        void Update(GameTime gameTime);
    }
}
