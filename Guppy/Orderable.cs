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
        #endregion

        #region Constructor
        public Orderable()
        {
            this.Events.Register<Int32>("draw-order:changed");
            this.Events.Register<Int32>("update-order:changed");
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);
        }
        #endregion

        #region Helper Methods
        public void SetDrawOrder(Int32 value)
        {
            if (value != this.DrawOrder)
            {
                this.DrawOrder = value;

                this.Events.TryInvoke<Int32>(this, "draw-order:changed", this.DrawOrder);
            }
        }

        public void SetUpdateOrder(Int32 value)
        {
            if (value != this.UpdateOrder)
            {
                this.UpdateOrder = value;

                this.Events.TryInvoke<Int32>(this, "update-order:changed", this.UpdateOrder);
            }
        }
        #endregion
    }
}
