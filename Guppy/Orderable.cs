using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// Represents a driven object that can be ordered.
    /// 
    /// Mostly used via entities and layers.
    /// </summary>
    public class Orderable : Driven
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

            this.Events.Register<Int32>("changed:draw-order");
            this.Events.Register<Int32>("changed:update-order");
            this.Events.Register<Boolean>("changed:visible");
            this.Events.Register<Boolean>("changed:enabled");
        }
        #endregion

        #region Helper Methods
        public void SetDrawOrder(Int32 value)
        {
            if (value != this.DrawOrder)
            {
                this.DrawOrder = value;

                this.Events.TryInvoke<Int32>(this, "changed:draw-order", this.DrawOrder);
            }
        }

        public void SetUpdateOrder(Int32 value)
        {
            if (value != this.UpdateOrder)
            {
                this.UpdateOrder = value;

                this.Events.TryInvoke<Int32>(this, "changed:update-order", this.UpdateOrder);
            }
        }

        public void SetVisible(Boolean value)
        {
            if (value != this.Visible)
            {
                this.Visible = value;

                this.Events.TryInvoke<Boolean>(this, "changed:visible", this.Visible);
            }
        }

        public void SetEnabled(Boolean value)
        {
            if (value != this.Enabled)
            {
                this.Enabled = value;

                this.Events.TryInvoke<Boolean>(this, "changed:enabled", this.Enabled);
            }
        }
        #endregion
    }
}
