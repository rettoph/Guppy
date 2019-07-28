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
using Guppy.Network.Security;

namespace Guppy.Network
{
    public abstract class NetworkEntity : Entity
    {
        #region Private Fields
        private Boolean _dirty;
        private NetworkScene _networkScene;

        private Dictionary<String, Action<NetIncomingMessage>> _actionHandlers;
        #endregion

        #region Public Attributes
        public Boolean Dirty
        {
            get { return _dirty; }
            set
            {
                if (_dirty != value)
                {
                    _dirty = value;
                    this.Events.TryInvoke("changed:dirty", _dirty);
                }
            }
        }
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
            _actionHandlers = new Dictionary<String, Action<NetIncomingMessage>>();
        }
        #endregion

        #region Network Methods
        public void Read(NetIncomingMessage im)
        {
            this.logger.LogDebug($"Reading data to NetworkEntity<{this.GetType()}>({this.Id})");

            this.read(im);

            this.Events.TryInvoke("on:read", _dirty);
        }
        protected abstract void read(NetIncomingMessage im);

        public void Write(NetOutgoingMessage om)
        {
            this.write(om);

            this.Events.TryInvoke("on:write", _dirty);
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
            if(_actionHandlers.ContainsKey(type))
            {
                _actionHandlers[type].Invoke(im);
            }
            else
            {
                this.logger.LogWarning($"Unhandled network action => Type: {this.GetType().Name}, Action: {type}, Entity: {this.Id}");
            }
        }

        public void AddActionHandler(String action, Action<NetIncomingMessage> handler)
        {
            _actionHandlers[action] = handler;
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
        /// <summary>
        /// Create an action method directly bound to the current entity,
        /// specifically targeting a single user
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
        public NetOutgoingMessage CreateActionMessage(String type, NetConnection recipient)
        {
            var om = _networkScene.Group.CreateMessage("action", NetDeliveryMethod.ReliableOrdered, 0, recipient);
            om.Write(this.Id);
            om.Write(type);

            return om;
        }
        #endregion
    }
}
