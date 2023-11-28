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
    internal class GameSubscriptionComponent : GameLoopComponent, IDisposable
    {
        private readonly IInputService _inputs;

        private IGameLoop _gameLoop;

        public GameSubscriptionComponent(IInputService inputs)
        {
            _gameLoop = null!;
            _inputs = inputs;
        }

        public override void Initialize(IGameLoop gameLoop)
        {
            base.Initialize(gameLoop);

            _gameLoop = gameLoop;

            _inputs.SubscribeMany(_gameLoop.Components.OfType<IBaseSubscriber<IInput>>());
        }

        public void Dispose()
        {
            _inputs.UnsubscribeMany(_gameLoop.Components.OfType<IBaseSubscriber<IInput>>());
        }
    }
}
