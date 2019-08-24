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
        public Int32 DrawOrder { get; protected set; }
        public Int32 UpdateOrder { get; protected set; }
        public Boolean Visible { get; protected set; }
        public Boolean Enabled { get; protected set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Events.TryRegister<Int32>("changed:draw-order");
            this.Events.TryRegister<Int32>("changed:update-order");
            this.Events.TryRegister<Boolean>("changed:visible");
            this.Events.TryRegister<Boolean>("changed:enabled");
        }
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

        #region Helper Methods
        public void SetDrawOrder(Int32 value)
        {
            if (value != this.DrawOrder)
            {
                this.DrawOrder = value;

                this.Events.Invoke<Int32>("changed:draw-order", this, this.DrawOrder);
            }
        }

        public void SetUpdateOrder(Int32 value)
        {
            if (value != this.UpdateOrder)
            {
                this.UpdateOrder = value;

                this.Events.Invoke<Int32>("changed:update-order", this, this.UpdateOrder);
            }
        }

        public void SetVisible(Boolean value)
        {
            if (value != this.Visible)
            {
                this.Visible = value;

                this.Events.Invoke<Boolean>("changed:visible", this, this.Visible);
            }
        }

        public void SetEnabled(Boolean value)
        {
            if (value != this.Enabled)
            {
                this.Enabled = value;

                this.Events.Invoke<Boolean>("changed:enabled", this, this.Enabled);
            }
        }
        #endregion
    }
}
