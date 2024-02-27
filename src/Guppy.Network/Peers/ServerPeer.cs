using Autofac;
using Guppy.Network.Definitions;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Groups;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Services;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using LiteNetLib;

namespace Guppy.Network.Peers
{
    internal class ServerPeer : Peer, IServerPeer
    {
        public override PeerType Type => PeerType.Server;

        public delegate bool ConnectionApprovalDelegate(ConnectionRequest request, INetIncomingMessage<UserAction> data);

        public event ConnectionApprovalDelegate? ConnectionApproval;

        public ServerPeer(ILifetimeScope scope, INetSerializerProvider serializers, IEnumerable<NetMessageTypeDefinition> messages) : base(scope, serializers, messages)
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

        protected override INetGroup GroupFactory(byte id)
        {
            return new ServerNetGroup(id, this);
        }

        private void HandleUserConnected(IUserService sender, User newUser)
        {
            this.Group.Users.Add(newUser);
        }

        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            using (INetIncomingMessage data = this.Messages.Read(request.Data, 0, DeliveryMethod.ReliableOrdered))
            {
                if (data is INetIncomingMessage<UserAction> casted)
                {
                    bool accepted = true;

                    if (this.ConnectionApproval is not null)
                    {
                        foreach (ConnectionApprovalDelegate del in this.ConnectionApproval.GetInvocationList())
                        {
                            accepted &= del(request, casted);
                        }
                    }

                    if (accepted)
                    {
                        NetPeer peer = request.Accept();

                        User user = this.Users.Create(peer.Id, peer, casted.Body.Claims);

                        this.Group.CreateMessage(user.CreateAction(UserActionTypes.CurrentUserConnected, ClaimAccessibility.Protected))
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
