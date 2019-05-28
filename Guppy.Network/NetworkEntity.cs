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
    public abstract class NetworkEntity : Entity, ITrackedNetworkObject
    {
        private Boolean _dirty;
        private NetworkScene _networkScene;
        public Dictionary<String, Action<NetIncomingMessage>> ActionHandlers { get; private set; }

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

        public event EventHandler<ITrackedNetworkObject> OnDirtyChanged;

        public NetworkEntity(EntityConfiguration configuration, Scene scene, IServiceProvider provider, ILogger logger) : base(configuration, scene, provider, logger)
        {
        }

        public NetworkEntity(Guid id, EntityConfiguration configuration, Scene scene, IServiceProvider provider, ILogger logger) : base(id, configuration, scene, provider, logger)
        {
        }

        protected override void Boot()
        {
            base.Boot();

            _dirty = false;
            _networkScene = this.scene as NetworkScene;
            this.ActionHandlers = new Dictionary<String, Action<NetIncomingMessage>>();
        }

        public virtual void Read(NetIncomingMessage im)
        {
            //
        }

        public virtual void Write(NetOutgoingMessage om)
        {
            //
        }

        public void HandleAction(String type, NetIncomingMessage im)
        {
            if(this.ActionHandlers.ContainsKey(type))
            {
                this.ActionHandlers[type].Invoke(im);
            }
            else
            {
                this.logger.LogWarning($"Unhandled network action => Type: {this.GetType().Name}, Action: {type}, Entity: {this.Id}");
            }
        }

        public NetOutgoingMessage CreateActionMessage(String type)
        {
            var om = _networkScene.group.CreateMessage("action");
            om.Write(this.Id);
            om.Write(type);
            _networkScene.actionQueue.Enqueue(om);

            return om;
        }

        public NetOutgoingMessage BuildCreateMessage()
        {
            var om = _networkScene.group.CreateMessage("create");
            om.Write(this.Configuration.Handle);
            om.Write(this.Id);

            return om;
        }

        public NetOutgoingMessage BuildUpdateMessage()
        {
            var om = _networkScene.group.CreateMessage("update");
            om.Write(this.Id);
            this.Write(om);

            return om;
        }
    }
}
