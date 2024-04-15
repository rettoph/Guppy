using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging;
using Guppy.Engine.Common.Components;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
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
