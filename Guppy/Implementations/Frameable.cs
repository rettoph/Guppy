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

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Events.TryRegisterDelegate<Int32>("changed:draw-order");
            this.Events.TryRegisterDelegate<Int32>("changed:update-order");
            this.Events.TryRegisterDelegate<Boolean>("changed:visible");
            this.Events.TryRegisterDelegate<Boolean>("changed:enabled");
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

                this.Events.Invoke<Int32>("changed:draw-order", this.DrawOrder);
            }
        }

        public void SetUpdateOrder(Int32 value)
        {
            if (value != this.UpdateOrder)
            {
                this.UpdateOrder = value;

                this.Events.Invoke<Int32>("changed:update-order", this.UpdateOrder);
            }
        }

        public void SetVisible(Boolean value)
        {
            if (value != this.Visible)
            {
                this.Visible = value;

                this.Events.Invoke<Boolean>("changed:visible", this.Visible);
            }
        }

        public void SetEnabled(Boolean value)
        {
            if (value != this.Enabled)
            {
                this.Enabled = value;

                this.Events.Invoke<Boolean>("changed:enabled", this.Enabled);
            }
        }
        #endregion
    }
}
