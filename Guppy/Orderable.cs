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

        #region Events
        public event EventHandler<Int32> OnDrawOrderChanged;
        public event EventHandler<Int32> OnUpdateOrderChanged;
        #endregion

        #region Helper Methods
        public void SetDrawOrder(Int32 value)
        {
            if (value != this.DrawOrder)
            {
                this.DrawOrder = value;

                this.OnDrawOrderChanged?.Invoke(this, this.DrawOrder);
            }
        }

        public void SetUpdateOrder(Int32 value)
        {
            if (value != this.UpdateOrder)
            {
                this.UpdateOrder = value;

                this.OnUpdateOrderChanged?.Invoke(this, this.UpdateOrder);
            }
        }
        #endregion
    }
}
