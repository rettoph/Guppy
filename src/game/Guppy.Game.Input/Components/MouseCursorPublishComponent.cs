using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Constants;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Components
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    [Sequence<UpdateSequence>(UpdateSequence.PreUpdate)]
    internal sealed class MouseCursorPublishComponent : EngineComponent, IGuppyUpdateable
    {
        private readonly IInputService _inputs;
        private readonly ICursor _cursor;

        public MouseCursorPublishComponent(ICursorService cursors, IInputService inputs)
        {
            _inputs = inputs;
            _cursor = cursors.Get(Cursors.Mouse);
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

            if (_cursor.SetPress(CursorButtons.Middle, state.MiddleButton == ButtonState.Pressed, out press))
            {
                _inputs.Publish(press);
            }

            if (_cursor.SetPress(CursorButtons.Right, state.RightButton == ButtonState.Pressed, out press))
            {
                _inputs.Publish(press);
            }
        }
    }
}
