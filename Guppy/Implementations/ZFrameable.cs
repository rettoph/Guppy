using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public abstract class ZFrameable : Driven, IZFrameable
    {
        private Boolean _visible;
        private Boolean _enabled;
        private Int32 _drawOrder;
        private Int32 _updateOrder;

        public Boolean Visible
        {
            get { return _visible; }
        }
        public Boolean Enabled
        {
            get { return _enabled; }
        }
        public Int32 DrawOrder
        {
            get { return _drawOrder; }
        }
        public Int32 UpdateOrder
        {
            get { return _updateOrder; }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        

        public ZFrameable(IServiceProvider provider) : base(provider)
        {
            this.SetDrawOrder(0);
            this.SetUpdateOrder(0);

            this.SetVisible(true);
            this.SetEnabled(true);
        }
        public ZFrameable(Guid id, IServiceProvider provider) : base(id, provider)
        {
            this.SetDrawOrder(0);
            this.SetUpdateOrder(0);

            this.SetVisible(true);
            this.SetEnabled(true);
        }

        public void SetEnabled(Boolean enabled)
        {
            _enabled = enabled;
            this.EnabledChanged?.Invoke(this, null);
        }

        public void SetVisible(Boolean visible)
        {
            _visible = visible;
            this.VisibleChanged?.Invoke(this, null);
        }

        public void SetUpdateOrder(Int32 updateOrder)
        {
            _updateOrder = updateOrder;
            this.UpdateOrderChanged?.Invoke(this, null);
        }

        public void SetDrawOrder(Int32 drawOrder)
        {
            _drawOrder = drawOrder;
            this.DrawOrderChanged?.Invoke(this, null);
        }
    }
}
