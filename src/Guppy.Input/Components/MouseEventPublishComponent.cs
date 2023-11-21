using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Input.Constants;
using Guppy.Input.Enums;
using Guppy.Input.Messages;
using Guppy.Input.Providers;
using Guppy.MonoGame;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Components
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PreUpdate)]
    internal sealed class MouseEventPublishComponent : IGuppyComponent, IUpdateableComponent
    {
        private readonly IBus _bus;
        private readonly Cursor _cursor;

        public MouseEventPublishComponent(ICursorProvider cursors, IBus bus)
        {
            _bus = bus;
            _cursor = (cursors.Get(Cursors.Mouse) as Cursor)!;
        }

        public void Initialize(IGuppy guppy)
        {
            //
        }

        public void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            if (_cursor.MoveTo(state.Position.ToVector2(), out var movement))
            {
                _bus.Enqueue(movement);
            }

            if (_cursor.ScrollTo(state.ScrollWheelValue, out var scrolling))
            {
                _bus.Enqueue(scrolling);
            }

            if (_cursor.SetPress(CursorButtons.Left, state.LeftButton == ButtonState.Pressed, out var press))
            {
                _bus.Enqueue(press);
            }

            if (_cursor.SetPress(CursorButtons.Middle, state.LeftButton == ButtonState.Pressed, out press))
            {
                _bus.Enqueue(press);
            }

            if (_cursor.SetPress(CursorButtons.Right, state.LeftButton == ButtonState.Pressed, out press))
            {
                _bus.Enqueue(press);
            }
        }
    }
}
