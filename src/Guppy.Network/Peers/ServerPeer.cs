using Autofac;
using Guppy.Network.Definitions;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Groups;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using LiteNetLib;

namespace Guppy.Network.Peers
{
    internal class ServerPeer : Peer, IServerPeer
    {
        public override PeerType Type => PeerType.Server;

        public delegate bool ConnectionApprovalDelegate(ConnectionRequest request, INetIncomingMessage<ConnectionRequestData> data);

        public event ConnectionApprovalDelegate? ConnectionApproval;

        public ServerPeer(ILifetimeScope scope, INetSerializerProvider serializers, IEnumerable<NetMessageTypeDefinition> messages) : base(scope, serializers, messages)
        {
            this.Listener.ConnectionRequestEvent += this.HandleConnectionRequestEvent;
            this.Listener.PeerDisconnectedEvent += this.HandlePeerDisconnectedEvent;
        }

        public void Start(int port, params Claim[] claims)
        {
            this.Users.Current = this.Users.Create(null, claims, Claim.Public(UserType.Server));

            base.Start();

            this.Manager.Start(port);
        }

        protected override INetGroup GroupFactory(byte id)
        {
            return new ServerNetGroup(id, this);
        }

        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            using (INetIncomingMessage data = this.Messages.Read(null!, request.Data, 0, DeliveryMethod.ReliableOrdered))
            {
                if (data is INetIncomingMessage<ConnectionRequestData> casted)
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

                        User user = this.Users.Create(peer, casted.Body.Claims, Claim.Public(UserType.User));

                        this.Group.CreateMessage(new ConnectionRequestResponse()
                        {
                            Type = ConnectionRequestResponseType.Accepted,
                            SystemUser = this.Users.Current!.ToDto(ClaimAccessibility.Public),
                            CurrentUser = user.ToDto(ClaimAccessibility.Protected)
                        }).AddRecipient(peer).Send().Recycle();

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
