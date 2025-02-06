using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Constants;
using Guppy.Game.Input.Common.Enums;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.Input.Systems
{
    public sealed class MouseCursorPublishSystem(ICursorService cursors, IInputService inputs) : IEngineSystem, IUpdateSystem
    {
        private readonly IInputService _inputs = inputs;
        private readonly ICursor _cursor = cursors.Get(Cursors.Mouse);

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.PreUpdate)]
        public void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            if (this._cursor.MoveTo(state.Position.ToVector2(), out var movement))
            {
                this._inputs.Publish(movement);
            }

            if (this._cursor.ScrollTo(state.ScrollWheelValue, out var scrolling))
            {
                this._inputs.Publish(scrolling);
            }

            if (this._cursor.SetPress(CursorButtonsEnum.Left, state.LeftButton == ButtonState.Pressed, out var press))
            {
                this._inputs.Publish(press);
            }

            if (this._cursor.SetPress(CursorButtonsEnum.Middle, state.MiddleButton == ButtonState.Pressed, out press))
            {
                this._inputs.Publish(press);
            }

            if (this._cursor.SetPress(CursorButtonsEnum.Right, state.RightButton == ButtonState.Pressed, out press))
            {
                this._inputs.Publish(press);
            }
        }
    }
}