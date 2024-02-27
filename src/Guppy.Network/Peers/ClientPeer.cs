using Autofac;
using Guppy.Messaging;
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
    internal class ClientPeer : Peer, IClientPeer, ISubscriber<INetIncomingMessage<UserAction>>
    {
        private NetPeer? _peer;

        public override PeerType Type => PeerType.Client;

        public ClientPeer(ILifetimeScope scope, INetSerializerProvider serializers, IEnumerable<NetMessageTypeDefinition> messages) : base(scope, serializers, messages)
        {
        }

        public new void Start()
        {
            base.Start();

            this.Manager.Start();
        }

        public void Connect(string address, int port, params Claim[] claims)
        {
            User user = new User(-1, claims);
            UserAction action = user.CreateAction(UserActionTypes.ConnectionRequest, ClaimAccessibility.Protected);

            using (var request = this.Group.CreateMessage(in action))
            {
                _peer = this.Manager.Connect(address, port, request.Writer);
            }
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
        {
            switch (message.Body.Type)
            {
                case UserActionTypes.Connected:
                    this.Users.UpdateOrCreate(message.Body.Id, message.Body.Claims);
                    break;
                case UserActionTypes.CurrentUserConnected:
                    this.Users.Current = this.Users.UpdateOrCreate(message.Body.Id, message.Body.Claims);
                    break;
            }
        }

        protected override void Send(INetOutgoingMessage message)
        {
            if (message.Recipients.Count == 0)
            {
                _peer!.Send(message.Writer, message.OutgoingChannel, message.DeliveryMethod);
            }

            base.Send(message);
        }

        protected override INetGroup GroupFactory(byte id)
        {
            return new ClientNetGroup(id, this);
        }
    }
}
