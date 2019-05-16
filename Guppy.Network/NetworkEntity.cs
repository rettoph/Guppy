using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Peers;
using Guppy.Network.Groups;

namespace Guppy.Network
{
    public abstract class NetworkEntity : Entity, INetworkObject
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

        public NetworkEntity(EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
        }

        public NetworkEntity(Guid id, EntityConfiguration configuration, Scene scene, ILogger logger) : base(id, configuration, scene, logger)
        {
        }

        public virtual void Read(NetIncomingMessage im)
        {
            //
        }

        public virtual void Write(NetOutgoingMessage om)
        {
            // Write the entities id
            om.Write(this.Id);
        }

        public NetOutgoingMessage BuildCreateMessage(Group group)
        {
            var om = group.CreateMessage("create");
            om.Write(this.Configuration.Handle);
            this.Write(om);

            return om;
        }

        public NetOutgoingMessage BuildUpdateMessage(Group group)
        {
            var om = group.CreateMessage("update");
            this.Write(om);

            return om;
        }
    }
}
