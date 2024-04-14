using Guppy.Engine;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Messaging;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Components.Guppy
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PostUpdate)]
    internal class BusComponent : GuppyComponent, IGuppyUpdateable
    {
        private readonly IBus _bus;

        public BusComponent(IBus bus)
        {
            _bus = bus;
        }

        public void Update(GameTime gameTime)
        {
            _bus.Flush();
        }
    }
}
