using Guppy.Attributes;
using Guppy.Commands.Messages;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Common.Extensions;
using Guppy.Input;
using Guppy.Input.Services;
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
    internal sealed class GuppySubscriptionComponent : IGuppyComponent, IUpdateableComponent, ISequenceable<UpdateSequence>, IDisposable
    {
        private IGuppy _guppy;
        private readonly IBus _bus;
        private readonly IInputService _inputs;

        public GuppySubscriptionComponent(IBus bus, IInputService inputs)
        {
            _guppy = null!;
            _bus = bus;
            _inputs = inputs;
        }

        public void Initialize(IGuppy guppy)
        {
            _guppy = guppy;
            _bus.SubscribeMany(guppy.Components.OfType<IBaseSubscriber<IMessage>>());
            _inputs.SubscribeMany(guppy.Components.OfType<IBaseSubscriber<IInput>>());
        }

        public void Dispose()
        {
            _bus.SubscribeMany(_guppy.Components.OfType<IBaseSubscriber<IMessage>>());
            _inputs.UnsubscribeMany(_guppy.Components.OfType<IBaseSubscriber<IInput>>());
        }

        public void Update(GameTime gameTime)
        {
            _bus.Flush();
        }
    }
}
