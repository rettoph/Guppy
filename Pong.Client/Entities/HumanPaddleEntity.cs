using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pong.Client.Scenes;

namespace Pong.Client.Entities
{
    public class HumanPaddleEntity : PaddleEntity
    {
        public HumanPaddleEntity(PongScene scene) : base(scene)
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.W))
            {
                this.Bounds.Y += -0.05f;
            }

            if (kState.IsKeyDown(Keys.S))
            {
                this.Bounds.Y += 0.05f;
            }

            base.Update(gameTime);
        }
    }
}
