using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Messages;
using Guppy.Network.Enums;
using Guppy.Network.Security;
using Guppy.Network.Security.Dtos;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Services;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class ServerPeer : Peer
    {
        #region Private Fields
        private UserService _users;
        #endregion

        #region Events
        public event ValidateEventDelegate<ServerPeer, IEnumerable<Claim>> ValidateUserConnectionRequest;
        #endregion

        #region Lifecycle Methods
        protected override void PreCreate(ServiceProvider provider)
        {
            base.PreCreate(provider);

            provider.Settings.Set(NetworkAuthorization.Master);
        }
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.listener.ConnectionRequestEvent += this.HandleConnectionRequestEvent;
            this.listener.PeerDisconnectedEvent += this.HandlePeerDisconnectedEvent;
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _users);
        }

        protected override void Release()
        {
            base.Release();

            _users = default;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.listener.ConnectionRequestEvent -= this.HandleConnectionRequestEvent;
            this.listener.PeerDisconnectedEvent -= this.HandlePeerDisconnectedEvent;
        }
        #endregion

        #region Start Methods
        /// <summary>
        /// Start logic thread and listening on selected port
        /// </summary>
        /// <param name="port">port to listen</param>
        public Task TryStart(Int32 port, IEnumerable<Claim> claims, Int32 period = 16)
        {
            // Create a new local user representing the server
            this.CurrentUser = _users.UpdateOrCreate(-1, claims);

            this.manager.Start(port);

            return base.TryStart(period);
        }
        #endregion

        #region Event Handlers
        private void HandlePeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            // Attempt to automatically dispose the old user...
            if (_users.TryGetById(peer.Id, out User user))
            {
                user.Dispose();
            }
        }

        /// <summary>
        /// Vallidate an incoming connection request.
        /// </summary>
        /// <param name="request"></param>
        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            NetworkMessage message = this.network.ReadMessage(request.Data);
            ConnectionRequestMessage connectionRequestDto = message.Data as ConnectionRequestMessage;

            if(this.network.GetConfigurationHash() != connectionRequestDto.NetworkProviderConfigurationHash)
            {
                request.Reject();
                return;
            }


            if (this.ValidateUserConnectionRequest.Validate(this, connectionRequestDto.Claims, true))
            { // Connection has been approved...
                NetPeer client = request.Accept();

                // Create a new user instance...
                User user = _users.UpdateOrCreate(client.Id, connectionRequestDto.Claims);

                // Send an accepted response to peer...
                this.SendMessage(
                    new ConnectionRequestResponseMessage()
                    {
                        Accepted = true,
                        User = user.GetDto(ClaimType.Protected)
                    },
                    client);

                return;
            }

            request.Reject();
        }
        #endregion
    }
}
