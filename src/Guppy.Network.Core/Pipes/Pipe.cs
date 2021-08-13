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

        #region Events
        public event IPipe.NetworkEnityAddedToPipeDelegate OnNetworkEnityAddedToPipe;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            this.Messages = new MessageManager();
            this.Messages.Signer = _channel.SignMessage;
        }

        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<UserList>(Guppy.Network.Constants.ServiceConfigurations.TransientUserList);
            this.NetworkEntities = provider.GetService<ServiceList<INetworkEntity>>();

            this.NetworkEntities.OnAdded += this.HandleNetworkEntityAdded;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.NetworkEntities.OnAdded -= this.HandleNetworkEntityAdded;

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

        #region Event Handlers
        private void HandleNetworkEntityAdded(IServiceList<INetworkEntity> sender, INetworkEntity entity)
        {
            // Remove the entity from its old pipe
            entity.Pipe?.NetworkEntities.TryRemove(entity);

            IPipe oldPipe = entity.Pipe;
            entity.Pipe = this;

            this.OnNetworkEnityAddedToPipe?.Invoke(this, entity, oldPipe);
        }
        #endregion
    }
}
