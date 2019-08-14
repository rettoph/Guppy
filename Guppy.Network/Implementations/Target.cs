using Guppy.Collections;
using Guppy.Implementations;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.Authentication;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network.Implementations
{
    public abstract class Target : Driven, ITarget
    {
        #region Private Fields
        private NetPeer _peer;
        private Pool<NetOutgoingMessageConfiguration> _outgoingMessageConfigurationPool;
        #endregion

        #region Constructor
        public Target(NetPeer peer)
        {
            _peer = peer;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Save an outgoing message configuration...
            _outgoingMessageConfigurationPool = provider.GetService<Pool<NetOutgoingMessageConfiguration>>();

            // Register useful events...
            this.Events.TryRegisterDelegate<NetOutgoingMessageConfiguration>("created:message");
        }
        #endregion

        #region ITarget Implementation
        public abstract void SendMessage(NetOutgoingMessageConfiguration om);

        public NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChanel = 0, NetConnection target = null)
        {
            var config = _outgoingMessageConfigurationPool.Pull(this.provider, c =>
            {
                // Create a new message & write the peer data
                var om = _peer.CreateMessage();
                this.ConfigureMessage(om);
                om.Write(type);

                // Update the message configurations...
                c.Message = om;
                c.Method = method;
                c.SequenceChannel = sequenceChanel;
                c.Recipient = target;
            });

            // Invoke the created message event
            this.Events.Invoke<NetOutgoingMessageConfiguration>("created:message", config);

            return config.Message;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Configure an outgoing message for the current target.
        /// </summary>
        /// <param name="om"></param>
        protected abstract void ConfigureMessage(NetOutgoingMessage om);
        #endregion
    }
}
