using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions;
using Guppy.Interfaces;
using Guppy.Implementations;

namespace Guppy.Network
{
    public class NetworkObject : TrackedDisposable, INetworkObject
    {
        public Guid Id { get; private set; }

        private Boolean _dirty;
        public Boolean Dirty
        {
            get { return _dirty; }
            set
            {
                if (_dirty != value)
                {
                    _dirty = value;
                    this.OnDirtyChanged?.Invoke(this, this);
                }
            }
        }

        public event EventHandler<INetworkObject> OnDirtyChanged;

        #region Constructors
        public NetworkObject(Guid id)
        {
            this.Id = id;
        }
        public NetworkObject()
        {
            this.Id = Guid.NewGuid();
        }
        #endregion

        public virtual void Read(NetIncomingMessage im)
        {
            //
        }

        public virtual void Write(NetOutgoingMessage om)
        {
            // Write the network object's id
            om.Write(this.Id);
        }
    }
}
