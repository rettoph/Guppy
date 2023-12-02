using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Input.Constants;
using Guppy.Input.Enums;
using Guppy.Input.Messages;
using Guppy.Input.Providers;
using Guppy.Input.Services;
using Guppy.Game;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Game.Common;

namespace Guppy.Input.Components
{
    [AutoLoad]
    [Sequence<UpdateSequence>(UpdateSequence.PreUpdate)]
    internal sealed class MousePublishComponent : GlobalComponent, IGuppyUpdateable
    {
        private readonly IInputService _inputs;
        private readonly Cursor _cursor;

        public MousePublishComponent(ICursorProvider cursors, IInputService inputs)
        {
            _inputs = inputs;
            _cursor = (cursors.Get(Cursors.Mouse) as Cursor)!;
        }

        public void Initialize()
        {
        }

        public void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            if (_cursor.MoveTo(state.Position.ToVector2(), out var movement))
            {
                _inputs.Publish(movement);
            }

            if (_cursor.ScrollTo(state.ScrollWheelValue, out var scrolling))
            {
                _inputs.Publish(scrolling);
            }

            if (_cursor.SetPress(CursorButtons.Left, state.LeftButton == ButtonState.Pressed, out var press))
            {
                _inputs.Publish(press);
            }

            if (_cursor.SetPress(CursorButtons.Middle, state.LeftButton == ButtonState.Pressed, out press))
            {
                _inputs.Publish(press);
            }

            if (_cursor.SetPress(CursorButtons.Right, state.LeftButton == ButtonState.Pressed, out press))
            {
                _inputs.Publish(press);
            }
        }
    }
}
