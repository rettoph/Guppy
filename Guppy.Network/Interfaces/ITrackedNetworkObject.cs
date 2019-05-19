using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface ITrackedNetworkObject : IUniqueObject, INetworkObject
    {
        Boolean Dirty { get; set; }
        event EventHandler<ITrackedNetworkObject> OnDirtyChanged;
    }
}
