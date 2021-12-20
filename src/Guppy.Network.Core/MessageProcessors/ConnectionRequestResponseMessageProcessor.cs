using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Messages;
using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Guppy.Network.Security.Services;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Threading;

namespace Guppy.Network.MessageProcessors
{
    internal sealed class ConnectionRequestResponseMessageProcessor : MessageProcessor<ConnectionRequestResponseMessage>
    {
        private ClientPeer _client;
        private UserService _users;

        protected override void Initialize(ServiceProvider provider)
        {
            provider.Service(out _client);
            provider.Service(out _users);
        }

        protected override void Release()
        {
            base.Release();

            _client = default;
            _client = default;
        }

        #region Lifecycle Methods
        public override void Process(ConnectionRequestResponseMessage message)
        {
            _client.CurrentUser = _users.UpdateOrCreate(message.User);
        }
        #endregion
    }
}
