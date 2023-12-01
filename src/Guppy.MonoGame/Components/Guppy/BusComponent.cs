using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Messaging;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components.Guppy
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PostUpdate)]
    internal class BusComponent : GuppyComponent, IUpdateableComponent
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
