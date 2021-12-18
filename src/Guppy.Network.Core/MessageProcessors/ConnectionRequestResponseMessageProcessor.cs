using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Dtos;
using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Guppy.Network.Security.Services;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.MessageProcessors
{
    internal sealed class ConnectionRequestResponseMessageProcessor : MessageProcessor<ConnectionRequestResponseDto>
    {
        private ClientPeer _client;
        private UserService _users;

        private ConnectionRequestResponseMessageProcessor(ServiceProvider provider)
        {
            provider.Service(out _client);
            provider.Service(out _users);
        }

        protected override void Process(ConnectionRequestResponseDto data, Message message)
        {
            _client.CurrentUser = _users.UpdateOrCreate(data.User);
        }

        public override void Dispose()
        {
            _client = null;
        }

        internal static ConnectionRequestResponseMessageProcessor Factory(ServiceProvider provider)
        {
            return new ConnectionRequestResponseMessageProcessor(provider);
        }
    }
}
