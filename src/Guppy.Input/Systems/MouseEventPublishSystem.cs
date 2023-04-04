using Guppy.Common;
using Guppy.Input.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Systems
{
    internal sealed class MouseEventPublishSystem : UpdateSystem
    {
        private readonly IBus _bus;
        private MouseMove? _previous;

        public MouseEventPublishSystem(IBus bus)
        {
            _bus = bus;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            MouseMove move = new MouseMove(state.Position.ToVector2(), _previous);

            if(move.Delta != Vector2.Zero)
            {
                _bus.Enqueue(move);
            }
        }
    }
}
