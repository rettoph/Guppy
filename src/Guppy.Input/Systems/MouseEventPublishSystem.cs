using Guppy.Common;
using Guppy.Input.Constants;
using Guppy.Input.Messages;
using Guppy.Input.Providers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Systems
{
    internal sealed class MouseEventPublishSystem : UpdateSystem
    {
        private readonly IBus _bus;
        private readonly MouseCursor _cursor;

        public MouseEventPublishSystem(ICursorProvider cursors, IBus bus)
        {
            _bus = bus;
            _cursor = (cursors.Get(Cursors.Mouse) as MouseCursor)!;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            
            if(_cursor.MoveTo(state.Position.ToVector2(), out var movement))
            {
                _bus.Enqueue(movement);
            }

            if(_cursor.ScrollTo(state.ScrollWheelValue, out var scrolling))
            {
                _bus.Enqueue(scrolling);
            }
        }
    }
}
