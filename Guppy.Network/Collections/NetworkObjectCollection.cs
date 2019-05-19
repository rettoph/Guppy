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
        public Queue<TNetworkObject> DirtyQueue;

        public NetworkObjectCollection(Boolean disposeOnRemove = true) : base(disposeOnRemove)
        {
            this.DirtyQueue = new Queue<TNetworkObject>();
        }

        public override void Add(TNetworkObject item)
        {
            base.Add(item);

            item.OnDirtyChanged += this.HandleDirtyChanged;
        }

        public override bool Remove(TNetworkObject item)
        {
            if(base.Remove(item))
            {
                item.OnDirtyChanged -= this.HandleDirtyChanged;

                return true;
            }

            return false;
        }

        #region Event Handlers
        private void HandleDirtyChanged(object sender, ITrackedNetworkObject e)
        {
            if(e.Dirty)
            { // Add the network object to the dirty queue
                this.DirtyQueue.Enqueue(e as TNetworkObject);
            }
        }
        #endregion
    }
}
