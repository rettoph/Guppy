using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Input;
using Guppy.Input.Services;
using Guppy.MonoGame.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    internal class GameSubscriptionComponent : GlobalComponent, IDisposable
    {
        private readonly IInputService _inputs;
        private IGlobalComponent[] _components;

        public GameSubscriptionComponent(IInputService inputs)
        {
            _inputs = inputs;
            _components = Array.Empty<IGlobalComponent>();
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            _components = components;

            _inputs.SubscribeMany(_components.OfType<IBaseSubscriber<IInput>>());
        }

        public void Dispose()
        {
            _inputs.UnsubscribeMany(_components.OfType<IBaseSubscriber<IInput>>());
        }
    }
}
