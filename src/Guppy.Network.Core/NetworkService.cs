using Guppy.DependencyInjection;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;

namespace Guppy.Network
{
    public abstract class NetworkService : Service, INetworkService
    {
        #region INetworkService Implementation
        public MessageManager Messages { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = new MessageManager(this.SignMessage, this.DefaultMessageFactory);
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.Messages = null;
        }
        #endregion

        #region Helper Methods
        protected virtual void SignMessage(NetOutgoingMessage om)
        {
            om.Write(this.Id);
        }

        /// <summary>
        /// Helper method for the internal creation of messages.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        protected abstract NetOutgoingMessage DefaultMessageFactory(NetOutgoingMessageContext context, NetConnection recipient, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter);
        #endregion
    }
}
