﻿using Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Services;
using LiteNetLib;

namespace Guppy.Core.Network.Peers
{
    internal abstract class Peer : IPeer, IDisposable
    {
        private readonly IMessageBus _bus;

        public readonly EventBasedNetListener Listener;
        public readonly NetManager Manager;
        public abstract PeerTypeEnum Type { get; }
        public PeerStateEnum State { get; private set; }
        public UserService Users { get; }

        public INetMessageService Messages { get; }
        public INetGroupService Groups { get; }
        public INetGroup Group { get; private set; }

        IUserService IPeer.Users => this.Users;

        public Peer(ILifetimeScope scope, INetSerializerService serializers, IEnumerable<NetMessageTypeDefinition> messages)
        {
            ILifetimeScope innerScope = scope.BeginLifetimeScope();
            this._bus = innerScope.Resolve<IMessageBus>();

            this.Listener = new EventBasedNetListener();
            this.Manager = new NetManager(this.Listener);
            this.Users = new UserService();
            this.Messages = new NetMessageService(this, serializers, messages);
            this.Groups = new NetGroupService(this.GroupFactory);
            this.Group = null!;
            this.State = PeerStateEnum.NotStarted;

            this.Listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;
        }

        protected virtual void Start()
        {
            this._bus.Subscribe(this);

            this.Group = this.Groups.GetById(NetScopeConstants.PeerScopeId);
            this.Group.Add(this._bus);

            this.State = PeerStateEnum.Started;
        }

        public void Flush()
        {
            this.Manager.PollEvents();

            this._bus.Flush();
        }

        private void HandleNetworkReceiveEvent(NetPeer sender, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            while (!reader.EndOfData)
            {
                this.Messages.Read(sender, reader, channel, deliveryMethod).Publish();
            }
        }

        public virtual void Send(INetOutgoingMessage message)
        {
            foreach (NetPeer recipient in message.Recipients)
            {
                recipient.Send(message.Writer, message.OutgoingChannel, message.DeliveryMethod);
            }
        }

        protected abstract INetGroup GroupFactory(byte id);
    }
}