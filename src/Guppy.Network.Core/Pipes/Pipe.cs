using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.Lists.Interfaces;
using Guppy.Network.Contexts;
using Guppy.Network.Extensions;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Pipes
{
    public class Pipe : UserList, IPipe
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
        public IChannel Channel => _channel;

        public UserList Users { get; private set; }

        public ServiceList<INetworkEntity> NetworkEntities { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = new MessageManager();
            this.Messages.Signer = _channel.SignMessage;
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<UserList>(Guppy.Network.Constants.ServiceConfigurations.TransientUserList);
            this.NetworkEntities = provider.GetService<ServiceList<INetworkEntity>>();

            this.Users.OnRemoved += this.HandleUserRemoved;
            this.NetworkEntities.OnRemoved += this.HandleNetworkEntityRemoved;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Users.OnRemoved -= this.HandleUserRemoved;
            this.NetworkEntities.OnRemoved -= this.HandleNetworkEntityRemoved;

            this.Users.TryRelease();
            this.NetworkEntities.TryRelease();

            this.Users = default;
            this.NetworkEntities = default;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.Messages.Dispose();
            this.Messages = null;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Automatically release the current pipe if
        /// no one or entity is residing within any longer.
        /// </summary>
        private void CheckPipe()
        {
            if (this.Users.Skip(1).Any())
                return;

            if (this.NetworkEntities.Any())
                return;

            this.TryRelease();
        }
        #endregion

        #region Event Handlers
        private void HandleUserRemoved(IServiceList<IUser> sender, IUser args)
            => this.CheckPipe();

        private void HandleNetworkEntityRemoved(IServiceList<INetworkEntity> sender, INetworkEntity args)
            => this.CheckPipe();
        #endregion
    }
}
