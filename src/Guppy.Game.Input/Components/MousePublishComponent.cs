using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input.Constants;
using Guppy.Game.Input.Enums;
using Guppy.Game.Input.Providers;
using Guppy.Game.Input.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Components
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
