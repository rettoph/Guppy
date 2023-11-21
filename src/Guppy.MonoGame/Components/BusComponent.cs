using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Common.Extensions;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PostUpdate)]
    internal sealed class BusComponent : IGuppyComponent, IUpdateableComponent, ISequenceable<UpdateSequence>
    {
        private readonly IBus _bus;

        public BusComponent(IBus bus)
        {
            _bus = bus;
        }

        public void Initialize(IGuppy guppy)
        {
            _bus.SubscribeMany(guppy.Components.OfType<ISubscriber>());
        }

        public void Update(GameTime gameTime)
        {
            _bus.Flush();
        }
    }
}
