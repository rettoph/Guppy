using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Frameable : Initializable
    {
        #region Public Attributes
        public Boolean Visible { get; protected set; }
        public Boolean Enabled { get; protected set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Visible = true;
            this.Enabled = true;

            this.Events.Register<Boolean>("visible:changed");
            this.Events.Register<Boolean>("enabled:changed");
        }
        #endregion

        #region Frame Methods
        public virtual void TryDraw(GameTime gameTime)
        {
            this.Draw(gameTime);
        }

        public virtual void TryUpdate(GameTime gameTime)
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

        #region Helper Methods
        public virtual void SetVisible(Boolean value)
        {
            if (value != this.Visible)
            {
                this.Visible = value;

                this.Events.TryInvoke<Boolean>(this, "visible:changed", this.Visible);
            }
        }

        public virtual void SetEnabled(Boolean value)
        {
            if (value != this.Enabled)
            {
                this.Enabled = value;

                this.Events.TryInvoke<Boolean>(this, "enabled:changed", this.Enabled);
            }
        }
        #endregion
    }
}
