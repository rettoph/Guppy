using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network
{
    public class NetworkEntity : Entity
    {
        #region Protected Attributes
        protected ITarget target { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize()
        {
            base.Initialize();

            // Load the current target value, if any, saved to the scope.
            this.target = this.provider.GetConfigurationValue<ITarget>("target");
        }
        #endregion

        #region Network Methods
        public NetOutgoingMessage CreateAction(String type, NetConnection recipient = null)
        {
            return this.CreateAction(type, this.target);
        }

        public NetOutgoingMessage CreateAction(String type, ITarget target, NetConnection recipient = null)
        {
            var om = target.CreateMessage("action", NetDeliveryMethod.UnreliableSequenced, 0, recipient);
            om.Write(this.Id);
            om.Write(type);

            return om;
        }

        public void TryRead(NetIncomingMessage im)
        {
            this.Read(im);
        }

        protected virtual void Read(NetIncomingMessage im)
        {
            // 
        }

        public void TryWrite(NetOutgoingMessage om)
        {
            this.Write(om);
        }

        protected virtual void Write(NetOutgoingMessage om)
        {
            // 
        }
        #endregion
    }
}
