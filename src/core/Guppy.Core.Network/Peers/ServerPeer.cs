using Autofac;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Extensions;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Enums;
using Guppy.Core.Network.Groups;
using Guppy.Core.Network.Messages;
using LiteNetLib;

namespace Guppy.Core.Network.Peers
{
    internal class ServerPeer : Peer, IServerPeer
    {
        private int _nextUserId;

        public override PeerType Type => PeerType.Server;

        public delegate bool ConnectionApprovalDelegate(ConnectionRequest request, INetIncomingMessage<ConnectionRequestData> data);

        public event ConnectionApprovalDelegate? ConnectionApproval;

        public ServerPeer(ILifetimeScope scope, INetSerializerService serializers, IEnumerable<NetMessageTypeDefinition> messages) : base(scope, serializers, messages)
        {
            this.Listener.ConnectionRequestEvent += this.HandleConnectionRequestEvent;
            this.Listener.PeerDisconnectedEvent += this.HandlePeerDisconnectedEvent;
        }

        public void Start(int port, params Claim[] claims)
        {
            this.Users.Current.Set(claims.Concat(Claim.Public(UserType.Server)));
            this.Users.Current.Initialize(_nextUserId++, null);

            base.Start();

            this.Manager.Start(port);
        }

        protected override INetGroup GroupFactory(byte id)
        {
            return new ServerNetGroup(id, this);
        }

        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            using INetIncomingMessage data = this.Messages.Read(null!, request.Data, 0, DeliveryMethod.ReliableOrdered);
            if (data is INetIncomingMessage<ConnectionRequestData> casted)
            {
                bool accepted = true;

                if (this.ConnectionApproval is not null)
                {
                    foreach (Delegate del in this.ConnectionApproval.GetInvocationList())
                    {
                        accepted &= ((ConnectionApprovalDelegate)del)(request, casted);
                    }
                }

                if (accepted)
                {
                    NetPeer peer = request.Accept();

                    IUser user = this.Users.Create(casted.Body.Claims, Claim.Public(UserType.User)).Initialize(_nextUserId++, peer);

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

        private void HandlePeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            this.Users.Remove(peer.Id);
        }
    }
}
