using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Frameable : Initializable
    {
        #region Frame Methods
        public void TryDraw(GameTime gameTime)
        {
            this.Draw(gameTime);
        }

        public void TryUpdate(GameTime gameTime)
        {
            this.Update(gameTime);
        }

        protected virtual void Draw(GameTime gameTime)
        {
            // 
        }

        protected virtual void Update(GameTime gameTime)
        {
            // 
        }
        #endregion
    }
}
