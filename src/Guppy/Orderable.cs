using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.System;
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
            set => this.OnVisibleChanged.InvokeIfChanged(value != _visible, this, ref _visible, value);
        }
        public Boolean Enabled
        {
            get => _enabled;
            set => this.OnEnabledChanged.InvokeIfChanged(value != _enabled, this, ref _enabled, value);
        }

        public Int32 DrawOrder
        {
            get => _drawOrder;
            set => this.OnDrawOrderChanged.InvokeIfChanged(value != _drawOrder, this, ref _drawOrder, value);
        }
        public Int32 UpdateOrder
        {
            get => _updateOrder;
            set => this.OnUpdateOrderChanged.InvokeIfChanged(value != _updateOrder, this, ref _updateOrder, value);
        }
        #endregion

        #region Events
        public event OnEventDelegate<Orderable, Int32> OnDrawOrderChanged;
        public event OnEventDelegate<Orderable, Int32> OnUpdateOrderChanged;
        public event OnEventDelegate<Orderable, Boolean> OnVisibleChanged;
        public event OnEventDelegate<Orderable, Boolean> OnEnabledChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Enabled = true;
            this.Visible = true;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

#if DEBUG_VERBOSE
            this.OnDrawOrderChanged.LogInvocationList($"OnDrawOrderChanged", this);
            this.OnUpdateOrderChanged.LogInvocationList($"OnUpdateOrderChanged", this);
            this.OnVisibleChanged.LogInvocationList($"OnVisibleChanged", this);
            this.OnEnabledChanged.LogInvocationList($"OnEnabledChanged", this);
#endif
        }
        #endregion
    }
}
