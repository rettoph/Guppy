using Guppy.Common;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using Guppy.Resources.Providers;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        public delegate bool ConnectionApprovalDelegate(ConnectionRequest request, INetIncomingMessage<UserAction> data);

        public event ConnectionApprovalDelegate? ConnectionApproval;

        public ServerPeer(
            ISettingProvider settings,
            INetScopeProvider scopes,
            IUserProvider users,
            IScoped<NetScope> scope,
            EventBasedNetListener listener,
            NetManager manager) : base(settings, scopes, users, scope, listener, manager)
        {
            this.Authorization = NetAuthorization.Master;

            this.listener.ConnectionRequestEvent += this.HandleConnectionRequestEvent;
            this.listener.PeerDisconnectedEvent += this.HandlePeerDisconnectedEvent;

            this.Users.OnUserConnected += this.HandleUserConnected;
        }

        public void Start(int port, params Claim[] claims)
        {
            base.Start();

            this.Users.Current = this.Users.UpdateOrCreate(-1, claims);

            this.manager.Start(port);
        }

        private void HandleUserConnected(IUserProvider sender, User newUser)
        {
            this.Scope.Users.Add(newUser);
        }

        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            var scopeId = request.Data.GetByte();

            if(scopeId != NetScopeConstants.PeerScopeId)
            {
                // request.Reject();
                throw new NotImplementedException();
            }

            using (var data = this.Scope.Read(null, request.Data, 0, DeliveryMethod.ReliableOrdered))
            {
                if (data is INetIncomingMessage<UserAction> casted)
                {
                    var accepted = true;

                    if(this.ConnectionApproval is not null)
                    {
                        foreach (ConnectionApprovalDelegate del in this.ConnectionApproval.GetInvocationList())
                        {
                            accepted &= del(request, casted);
                        }
                    }

                    if(accepted)
                    {
                        var peer = request.Accept();

                        var user = this.Users.UpdateOrCreate(peer.Id, peer, casted.Body.Claims);

                        this.Scope.Create(user.CreateAction(UserAction.Actions.CurrentUserConnected, ClaimAccessibility.Protected))
                            .AddRecipient(peer)
                            .Send()
                            .Recycle();

                        return;
                    }
                }

                request.Reject();
            }
        }

        private void HandlePeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            this.Users.Remove(peer.Id);
        }
    }
}
