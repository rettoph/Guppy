﻿using Autofac;
using Guppy.Messaging;
using Guppy.Network.Definitions;
using Guppy.Network.Enums;
using Guppy.Network.Groups;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Messages;
using Guppy.Network.Providers;

namespace Guppy.Network.Peers
{
    internal class ClientPeer : Peer, IClientPeer, ISubscriber<INetIncomingMessage<UserAction>>, ISubscriber<INetIncomingMessage<ConnectionRequestResponse>>
    {
        public override PeerType Type => PeerType.Client;

        public User? ServerUser { get; private set; }

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
            if (this.State == Enums.PeerState.NotStarted)
            {
                this.Start();
            }

            ConnectionRequestData requestData = new ConnectionRequestData()
            {
                Claims = claims
            };

            using (var request = this.Group.CreateMessage(in requestData))
            {
                this.Manager.Connect(address, port, request.Writer);
            }
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
        {
            switch (message.Body.Type)
            {
                case UserActionTypes.Connected:
                    this.Users.UpdateOrCreate(message.Body.UserDto);
                    break;
            }
        }

        protected override void Send(INetOutgoingMessage message)
        {
            if (message.Recipients.Count == 0)
            {
                this.ServerUser!.NetPeer!.Send(message.Writer, message.OutgoingChannel, message.DeliveryMethod);
            }

            base.Send(message);
        }

        protected override INetGroup GroupFactory(byte id)
        {
            return new ClientNetGroup(id, this);
        }

        public void Process(in Guid messageId, INetIncomingMessage<ConnectionRequestResponse> message)
        {
            if (message.Body.Type != ConnectionRequestResponseType.Accepted)
            {
                throw new InvalidOperationException();
            }

            this.ServerUser = this.Users.Create(message.Sender.Peer, message.Body.SystemUser!);
            this.Users.Current = this.Users.Create(null, message.Body.CurrentUser!);
        }
    }
}
