using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Interfaces;
using Guppy.Implementations;

namespace Guppy.Network
{
    public class NetworkObject : UniqueObject, INetworkObject
    {
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
        public NetworkObject()
        {
        }
        public NetworkObject(Guid id) : base(id)
        {
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
