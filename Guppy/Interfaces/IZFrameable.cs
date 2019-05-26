using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IZFrameable : IDriven
    {
        Boolean Enabled { get; }
        Int32 UpdateOrder { get; }

        event EventHandler<EventArgs> EnabledChanged;
        event EventHandler<EventArgs> UpdateOrderChanged;

        Int32 DrawOrder { get; }
        Boolean Visible { get; }

        event EventHandler<EventArgs> DrawOrderChanged;
        event EventHandler<EventArgs> VisibleChanged;

        void SetEnabled(Boolean enabled);
        void SetVisible(Boolean visible);

        void SetUpdateOrder(Int32 updateOrder);
        void SetDrawOrder(Int32 drawOrder);
    }
}
