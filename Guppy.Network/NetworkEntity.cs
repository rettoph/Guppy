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

        public event EventHandler<NetworkEntity> OnRead;
        public event EventHandler<NetworkEntity> OnWrite;

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

        public NetworkEntity(EntityConfiguration configuration, IServiceProvider provider) : base(configuration, provider)
        {
        }

        public NetworkEntity(Guid id, EntityConfiguration configuration, IServiceProvider provider) : base(id, configuration, provider)
        {
        }

        protected override void Boot()
        {
            base.Boot();

            _dirty = false;
            _networkScene = this.scene as NetworkScene;
            this.ActionHandlers = new Dictionary<String, Action<NetIncomingMessage>>();
        }

        public void Read(NetIncomingMessage im)
        {
            this.read(im);

            this.OnRead?.Invoke(this, this);
        }
        protected abstract void read(NetIncomingMessage im);

        public void Write(NetOutgoingMessage om)
        {
            this.write(om);

            this.OnWrite?.Invoke(this, this);
        }
        protected abstract void write(NetOutgoingMessage om);

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

        public NetOutgoingMessage CreateActionMessage(String type, Boolean priority = false)
        {
            var om = _networkScene.Group.CreateMessage("action");
            om.Write(this.Id);
            om.Write(type);

            if (priority)
                _networkScene.priorityActionQueue.Enqueue(om);
            else
                _networkScene.actionQueue.Enqueue(om);

            return om;
        }
    }
}
