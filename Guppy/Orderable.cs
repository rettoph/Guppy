using Guppy.Interfaces;
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
    public class Orderable : Driven, IOrderable
    {
        #region Private Fields
        private Int32 _drawOrder;
        private Int32 _updateOrder;
        #endregion

        #region Public Attributes
        public Int32 DrawOrder
        {
            get => _drawOrder;
            set
            {
                if(this.DrawOrder != value)
                {
                    _drawOrder = value;
                    this.OnDrawOrderChanged?.Invoke(this, _drawOrder);
                }
            }
        }
        public Int32 UpdateOrder
        {
            get => _updateOrder;
            set
            {
                if (this.UpdateOrder != value)
                {
                    _updateOrder = value;
                    this.OnUpdateOrderChanged?.Invoke(this, _updateOrder);
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<Int32> OnDrawOrderChanged;
        public event EventHandler<Int32> OnUpdateOrderChanged;
        #endregion
    }
}
