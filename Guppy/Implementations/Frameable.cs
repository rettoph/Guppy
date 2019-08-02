using Guppy.Enums;
using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public class Frameable : Initializable, IFrameable
    {
        #region Public Attributes
        public Int32 DrawOrder { get; protected set; }
        public Int32 UpdateOrder { get; protected set; }
        public Boolean Visible { get; protected set; }
        public Boolean Enabled { get; protected set; }
        #endregion

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
