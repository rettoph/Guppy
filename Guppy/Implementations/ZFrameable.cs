using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    /// <summary>
    /// A frameable object that can be enabled or given
    /// update & draw orders
    /// </summary>
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
        
        public ZFrameable(IServiceProvider provider) : base(provider)
        {
        }
        public ZFrameable(Guid id, IServiceProvider provider) : base(id, provider)
        {
        }

        protected override void Boot()
        {
            base.Boot();

            this.SetDrawOrder(0);
            this.SetUpdateOrder(0);

            this.SetVisible(true);
            this.SetEnabled(true);
        }

        public void SetEnabled(Boolean enabled)
        {
            _enabled = enabled;
            this.Events.TryInvoke("set:enabled", _enabled);
        }

        public void SetVisible(Boolean visible)
        {
            _visible = visible;
            this.Events.TryInvoke("set:visible", _visible);
        }

        public void SetUpdateOrder(Int32 updateOrder)
        {
            _updateOrder = updateOrder;
            this.Events.TryInvoke("set:update-order", _updateOrder);
        }

        public void SetDrawOrder(Int32 drawOrder)
        {
            _drawOrder = drawOrder;
            this.Events.TryInvoke("set:draw-order", _updateOrder);
        }
    }
}
