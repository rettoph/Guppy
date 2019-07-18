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
        #region Private Fields
        private Boolean _dirty;
        private NetworkScene _networkScene;
        #endregion

        #region Public Attributes
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
        #endregion

        #region Events
        public event EventHandler<NetworkEntity> OnRead;
        public event EventHandler<NetworkEntity> OnWrite;

        public event EventHandler<ITrackedNetworkObject> OnDirtyChanged;
        #endregion

        #region Constructors
        public NetworkEntity(EntityConfiguration configuration, IServiceProvider provider) : base(configuration, provider)
        {
        }

        public NetworkEntity(Guid id, EntityConfiguration configuration, IServiceProvider provider) : base(id, configuration, provider)
        {
        }
        #endregion

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            _dirty = false;
            _networkScene = this.scene as NetworkScene;
            this.ActionHandlers = new Dictionary<String, Action<NetIncomingMessage>>();
        }
        #endregion

        #region Network Methods
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
        #endregion

        #region Utility Methods
        /// <summary>
        /// Given a specific message type, run the action handler
        /// assigned.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="im"></param>
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
        #endregion

        #region Helper methods
        /// <summary>
        /// Create an action method directly bound to the current entity.
        /// 
        /// By default, actions are sent unreliably and unsequenced, but the
        /// priority flag can be set to ensure ordered delivery.
        /// 
        /// Actions are automatically added to the outgoing message buffer on
        /// creation.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public NetOutgoingMessage CreateActionMessage(String type, Boolean priority = false)
        {
            var om = _networkScene.Group.CreateMessage("action", priority ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.ReliableOrdered, 0);
            om.Write(this.Id);
            om.Write(type);

            return om;
        }
        #endregion
    }
}
