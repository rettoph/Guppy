using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Constants;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Components
{
    internal sealed class MouseCursorPublishComponent(ICursorService cursors, IInputService inputs) : IEngineComponent, IUpdatableComponent
    {
        private readonly IInputService _inputs = inputs;
        private readonly ICursor _cursor = cursors.Get(Cursors.Mouse);

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        [SequenceGroup<UpdateComponentSequenceGroup>(UpdateComponentSequenceGroup.PreUpdate)]
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
