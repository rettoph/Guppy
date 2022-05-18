using Guppy.EntityComponent.Services;
using Guppy.Network.Enums;
using Guppy.Network.Providers;
using Guppy.Network.Security;
using Guppy.Network.Security.Messages;
using Guppy.Network.Security.Providers;
using Guppy.Network.Security.Structs;
using Guppy.Providers;
using Guppy.Threading;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public sealed class ServerPeer : Peer
    {
        private EventBasedNetListener _listener;
        private NetManager _manager;

        public event ValidateEventDelegate<ServerPeer, ConnectionRequestMessage>? ValidateUserConnectionRequest;

        public ServerPeer(
            INetScopeProvider rooms,
            INetMessengerProvider messengers,
            IUserProvider users,
            ISettingProvider settings,
            EventBasedNetListener listener,
            NetManager manager,
            Bus bus,
            NetScope scope) : base(rooms, messengers, users, settings, listener, manager, scope)
        {
            _listener = listener;
            _manager = manager;

            _listener.ConnectionRequestEvent += this.HandleConnectionRequestEvent;

            settings.Get<NetworkAuthorization>().Value = NetworkAuthorization.Master;
        }

        public override void Dispose()
        {
            base.Dispose();

            _listener.ConnectionRequestEvent -= this.HandleConnectionRequestEvent;
        }

        /// <summary>
        /// Start logic thread and listening on selected port
        /// </summary>
        /// <param name="addressIPv4">bind to specific ipv4 address</param>
        /// <param name="addressIPv6">bind to specific ipv6 address</param>
        /// <param name="port">port to listen</param>
        public bool Start(IPAddress addressIPv4, IPAddress addressIPv6, int port, params Claim[] claims)
        {
            base.Start();

            this.CurrentUser = this.Users.UpdateOrCreate(-1, claims, null);
            this.Scope!.Users.TryJoin(this.CurrentUser);

            return _manager.Start(addressIPv4, addressIPv6, port);
        }

        /// <summary>
        /// Start logic thread and listening on selected port
        /// </summary>
        /// <param name="addressIPv4">bind to specific ipv4 address</param>
        /// <param name="addressIPv6">bind to specific ipv6 address</param>
        /// <param name="port">port to listen</param>
        public bool Start(string addressIPv4, string addressIPv6, int port, params Claim[] claims)
        {
            base.Start();

            this.CurrentUser = this.Users.UpdateOrCreate(-1, claims, null);
            this.Scope!.Users.TryJoin(this.CurrentUser);

            return _manager.Start(addressIPv4, addressIPv6, port);
        }

        /// <summary>
        /// Start logic thread and listening on selected port
        /// </summary>
        /// <param name="port">port to listen</param>
        public bool Start(int port, params Claim[] claims)
        {
            base.Start();

            this.CurrentUser = this.Users.UpdateOrCreate(-1, claims, null);
            this.Scope!.Users.TryJoin(this.CurrentUser);

            return _manager.Start(port);
        }

        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            ConnectionRequestMessage.Deserialize(request.Data, out var data);

            if(!this.ValidateUserConnectionRequest!.Validate(this, data, true))
            {
                request.Reject();
                return;
            }

            NetPeer client = request.Accept();
            User user = this.Users.UpdateOrCreate(client.Id, data.Claims, client);

            ConnectionResponseMessage response = new ConnectionResponseMessage(
                client.Id, 
                user.Claims.ToArray());

            this.Scope.Messenger!.Messages.CreateOutgoing<ConnectionResponseMessage>(in response)
                .AddRecipient(client)
                .Send()
                .Recycle();

            // Add the new user to the primary room.
            this.Scope.Users.TryJoin(user);
        }
    }
}
