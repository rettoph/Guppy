using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Dtos;
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
            this.listener.PeerConnectedEvent += this.HandlePeerConnectedEvent;
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
            this.listener.PeerConnectedEvent -= this.HandlePeerConnectedEvent;
        }
        #endregion

        #region Event Handlers
        private void HandlePeerConnectedEvent(NetPeer peer)
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Vallidate an incoming connection request.
        /// </summary>
        /// <param name="request"></param>
        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            ConnectionRequestDto connectionRequestDto = this.network.GetDataTypeConfiguration<ConnectionRequestDto>().Reader(request.Data) as ConnectionRequestDto;

            if(!this.network.CheckDto(connectionRequestDto.NetworkProvider))
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
                    Constants.Messages.ConnectionRequestResponse,
                    new ConnectionRequestResponseDto()
                    {
                        Accepted = true,
                        User = user.ToDto(ClaimType.Protected)
                    },
                    client);

                return;
            }

            request.Reject();
        }
        #endregion
    }
}
