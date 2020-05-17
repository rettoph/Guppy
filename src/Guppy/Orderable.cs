using Guppy.DependencyInjection;
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
    public abstract class Orderable : Driven
    {
        #region Private Fields
        private Boolean _visible;
        private Boolean _enabled;

        private Int32 _drawOrder;
        private Int32 _updateOrder;
        #endregion

        #region Public Attributes
        public Boolean Visible
        {
            get => _visible;
            set
            {
                if (value != this.Visible)
                {
                    _visible = value;
                    this.OnVisibleChanged?.Invoke(this, this.Visible);
                }
                    
            }
        }
        public Boolean Enabled
        {
            get => _enabled;
            set
            {
                if (value != this.Enabled) {
                    _enabled = value;
                    this.OnEnabledChanged?.Invoke(this, this.Enabled);
                }
            }
        }

        public Int32 DrawOrder
        {
            get => _drawOrder;
            set
            {
                if (this.DrawOrder != value)
                {
                    _drawOrder = value;
                    this.OnDrawOrderChanged?.Invoke(this, this.DrawOrder);
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
                    this.OnUpdateOrderChanged?.Invoke(this, this.UpdateOrder);
                }
            }
        }
        #endregion

        #region Events
        public event GuppyEventHandler<Orderable, Int32> OnDrawOrderChanged;
        public event GuppyEventHandler<Orderable, Int32> OnUpdateOrderChanged;
        public event GuppyEventHandler<Orderable, Boolean> OnVisibleChanged;
        public event GuppyEventHandler<Orderable, Boolean> OnEnabledChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Enabled = true;
            this.Visible = true;
        }
        #endregion
    }
}
