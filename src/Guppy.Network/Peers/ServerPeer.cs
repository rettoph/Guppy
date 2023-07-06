using Autofac;
using Guppy.Common;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Messages;
using LiteNetLib;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        public override PeerType Type => PeerType.Server;

        public delegate bool ConnectionApprovalDelegate(ConnectionRequest request, INetIncomingMessage<UserAction> data);

        public event ConnectionApprovalDelegate? ConnectionApproval;

        public ServerPeer(ILifetimeScope scope) : base(scope)
        {
            this.Listener.ConnectionRequestEvent += this.HandleConnectionRequestEvent;
            this.Listener.PeerDisconnectedEvent += this.HandlePeerDisconnectedEvent;

            this.Users.OnUserConnected += this.HandleUserConnected;
        }

        public void Start(int port, params Claim[] claims)
        {
            base.Start();

            this.Users.Current = this.Users.UpdateOrCreate(-1, claims);

            this.Manager.Start(port);
        }

        private void HandleUserConnected(IUserProvider sender, User newUser)
        {
            this.NetScope.Users.Add(newUser);
        }

        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            var scopeId = request.Data.GetByte();

            if(scopeId != NetScopeConstants.PeerScopeId)
            {
                // request.Reject();
                throw new NotImplementedException();
            }

            using (var data = this.NetScope.Messages.Read(null, request.Data, 0, DeliveryMethod.ReliableOrdered))
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

                        this.NetScope.Messages.Create(user.CreateAction(UserAction.Actions.CurrentUserConnected, ClaimAccessibility.Protected))
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
