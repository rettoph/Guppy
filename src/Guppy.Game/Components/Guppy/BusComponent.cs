using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Messaging;
using Guppy.Game;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Game.Common;

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
