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
using Guppy.Network.Utilities.DynamicDelegaters;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network
{
    public abstract class NetworkEntity : Entity
    {
        #region Private Fields
        private Boolean _dirty;
        private NetworkScene _networkScene;
        #endregion

        #region Public Attributes
        public ActionDelegater Actions { get; private set; }

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

            this.Actions = ActivatorUtilities.CreateInstance<ActionDelegater>(this.provider, this);
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
