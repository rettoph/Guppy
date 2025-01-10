using Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Enums;
using Guppy.Core.Network.Groups;
using Guppy.Core.Network.Messages;

namespace Guppy.Core.Network.Peers
{
    internal class ClientPeer(ILifetimeScope scope, INetSerializerService serializers, IEnumerable<NetMessageTypeDefinition> messages) : Peer(scope, serializers, messages), IClientPeer,
        ISubscriber<INetIncomingMessage<UserAction>>,
        ISubscriber<INetIncomingMessage<ConnectionRequestResponse>>
    {
        public override PeerTypeEnum Type => PeerTypeEnum.Client;

        public IUser? ServerUser { get; private set; }

        public new void Start()
        {
            base.Start();

            this.Manager.Start();
        }

        public void Connect(string address, int port, params Claim[] claims)
        {
            if (this.State == PeerStateEnum.NotStarted)
            {
                this.Start();
            }

            ConnectionRequestData requestData = new()
            {
                Claims = claims
            };

            using var request = this.Group.CreateMessage(in requestData);
            this.Manager.Connect(address, port, request.Writer);
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
        {
            IUser user = this.Users.UpdateOrCreate(message.Body.UserDto);

            switch (message.Body.Type)
            {
                case UserActionTypeEnum.UserJoined:
                    this.Groups.GetById(message.Body.GroupId).Users.Add(user);
                    break;
                case UserActionTypeEnum.UserLeft:
                    this.Groups.GetById(message.Body.GroupId).Users.Remove(user);
                    break;
            }
        }

        public override void Send(INetOutgoingMessage message)
        {
            if (message.Recipients.Count == 0)
            {
                this.ServerUser!.NetPeer!.Send(message.Writer, message.OutgoingChannel, message.DeliveryMethod);
            }

            base.Send(message);
        }

        protected override INetGroup GroupFactory(byte id) => new ClientNetGroup(id, this);

        public void Process(in Guid messageId, INetIncomingMessage<ConnectionRequestResponse> message)
        {
            if (message.Body.Type != ConnectionRequestResponseType.Accepted)
            {
                throw new InvalidOperationException();
            }

            this.ServerUser = this.Users.Create(message.Body.SystemUser!.Claims).Initialize(message.Body.SystemUser.Id, message.Sender.Peer);

            this.Users.Current.Set(message.Body.CurrentUser!.Claims);
            this.Users.Current.Initialize(message.Body.CurrentUser.Id, null);
        }
    }
}