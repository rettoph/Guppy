using Guppy.DependencyInjection;
using Guppy.Lists;
using Guppy.Network.Contexts;
using Guppy.Network.Extensions;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Pipes
{
    public class Pipe : Service, IPipe
    {
        #region Private Fields
        private IChannel _channel;
        #endregion

        #region INetworkService Implementation
        public MessageManager Messages { get; private set; }
        #endregion

        #region IPipe Implementation
        IChannel IPipe.Channel
        {
            get => _channel;
            set => _channel = value;
        }

        public UserList Users { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = new MessageManager(_channel.SignMessage);
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<UserList>(GuppyNetworkCoreConstants.ServiceConfigurations.TransientUserList);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Users.TryRelease();

            this.Users = null;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.Messages.Dispose();
            this.Messages = null;
        }
        #endregion

        #region IPipe Implementation
        public NetOutgoingMessage CreateMessage(NetOutgoingMessageContext context, NetConnection recipient = null, Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter = null)
            => _channel.CreateMessage(context, recipient, filter);
        #endregion
    }
}
