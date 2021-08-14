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

        private ServiceList<INetworkEntity> _networkEntities;
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

        public IEnumerable<INetworkEntity> NetworkEntities => _networkEntities;
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

            provider.Service(out _networkEntities);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Users.TryRelease();
            _networkEntities.TryRelease();

            this.Users = default;
            _networkEntities = default;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.Messages.Dispose();
            this.Messages = null;
        }
        #endregion

        #region IPipe Implementation
        void IPipe.TryAdd(INetworkEntity entity, IPipe oldPipe)
        {
            if(_networkEntities.TryAdd(entity))
            {
                this.OnNetworkEnityAddedToPipe?.Invoke(this, entity, oldPipe);
            }
        }

        void IPipe.TryRemove(INetworkEntity entity)
            => _networkEntities.TryRemove(entity);
        #endregion
    }
}
