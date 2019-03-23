using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.UI.Structs;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy.UI.Entities
{
    public class InputManager : Entity
    {
        public MouseData Mouse;

        public InputManager(EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
            this.SetVisible(false);

            this.Mouse = new MouseData();

            this.SetUpdateOrder(0);
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            var mState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            this.Mouse.Position = mState.Position.ToVector2();
            this.Mouse.LeftButton = mState.LeftButton;
        }
    }
}
