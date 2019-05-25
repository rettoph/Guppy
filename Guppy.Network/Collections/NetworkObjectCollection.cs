using Guppy.Collections;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class NetworkObjectCollection<TNetworkObject> : UniqueObjectCollection<TNetworkObject>
        where TNetworkObject : class, ITrackedNetworkObject
    {
        public NetworkObjectCollection(Boolean disposeOnRemove = true) : base(disposeOnRemove)
        {
        }
    }
}
