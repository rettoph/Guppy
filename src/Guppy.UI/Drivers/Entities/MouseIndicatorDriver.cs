using Guppy.DependencyInjection;
using Guppy.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Drivers.Entities
{
    public class MouseIndicatorDriver : Driver<Cursor>
    {
        #region Lifecycle Methods
        protected override void Configure(object driven, ServiceProvider provider)
        {
            base.Configure(driven, provider);

            this.driven.OnUpdate += this.Update;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.driven.OnUpdate -= this.Update;
        }
        #endregion

        #region Frame Methods
        private void Update(GameTime gameTime)
        {
            var mState = Mouse.GetState();
    
            // Move the pointer to the recieved mouse position
            this.driven.MoveTo(mState.Position.ToVector2());
    
            // Update the button states
            this.driven.SetButton(Cursor.Button.Left, mState.LeftButton == ButtonState.Pressed);
            this.driven.SetButton(Cursor.Button.Middle, mState.MiddleButton == ButtonState.Pressed);
            this.driven.SetButton(Cursor.Button.Right, mState.RightButton == ButtonState.Pressed);
    
            // Update the scroll value
            this.driven.ScrollTo(mState.ScrollWheelValue);
        }
        #endregion
    }
}
