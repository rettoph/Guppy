using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.UI.Structs;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Guppy.UI.Entities
{
    public class InputManager : Entity
    {
        public MouseData Mouse;
        private Int32 _lastScrollWheel;

        public InputManager(GameWindow window, EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
            this.SetVisible(false);

            this.Mouse = new MouseData();
            _lastScrollWheel = 0;

            this.SetUpdateOrder(0);
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            // Cache the mouse state...
            var mState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            var mPos = mState.Position.ToVector2();


            this.Mouse.Delta = mPos - this.Mouse.Position;
            this.Mouse.Position = mPos;
            this.Mouse.LeftButton = mState.LeftButton;
            this.Mouse.ScrollDelta = mState.ScrollWheelValue - _lastScrollWheel;

            _lastScrollWheel = mState.ScrollWheelValue;
        }
    }
}
