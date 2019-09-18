using Guppy.Attributes;
using Guppy.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Drivers
{
    [IsDriver(typeof(Pointer))]
    public class MousePointerDriver : Driver<Pointer>
    {
        public MousePointerDriver(Pointer driven) : base(driven)
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var mState = Mouse.GetState();

            // Move the pointer to the recieved mouse position
            this.driven.MoveTo(mState.Position.ToVector2());

            // Update the button states
            this.driven.SetButton(Pointer.Button.Left, mState.LeftButton == ButtonState.Pressed);
            this.driven.SetButton(Pointer.Button.Middle, mState.MiddleButton == ButtonState.Pressed);
            this.driven.SetButton(Pointer.Button.Right, mState.RightButton == ButtonState.Pressed);

            // Update the scroll value
            this.driven.ScrollTo(mState.ScrollWheelValue);
        }
    }
}
