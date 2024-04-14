using Autofac;
using Guppy.Engine.Common.Autofac;
using Guppy.Messaging;
using Guppy.Network.Constants;
using Guppy.Network.Definitions;
using Guppy.Network.Enums;
using Guppy.Network.Identity.Services;
using Guppy.Network.Services;
using LiteNetLib;

namespace Guppy.Network.Peers
{
    internal abstract class Peer : IDisposable, IPeer, IBaseSubscriber<IMessage>
    {
        private readonly IBus _bus;

        public readonly EventBasedNetListener Listener;
        public readonly NetManager Manager;
        public abstract PeerType Type { get; }
        public Enums.PeerState State { get; private set; }

        public IUserService Users { get; }
        public INetMessageService Messages { get; }
        public INetGroupService Groups { get; }
        public INetScope DefaultNetScope { get; }
        public INetGroup Group { get; private set; }

        public Peer(ILifetimeScope scope, INetSerializerService serializers, IEnumerable<NetMessageTypeDefinition> messages)
        {
            ILifetimeScope innerScope = scope.BeginLifetimeScope(LifetimeScopeTags.GuppyScope);
            _bus = innerScope.Resolve<IBus>();
            this.DefaultNetScope = innerScope.Resolve<INetScope>();

            this.Listener = new EventBasedNetListener();
            this.Manager = new NetManager(this.Listener);
            this.Users = new UserService();
            this.Messages = new NetMessageService(this, serializers, messages);
            this.Groups = new NetGroupService(this.GroupFactory);
            this.Group = null!;
            this.State = Enums.PeerState.NotStarted;

            this.Listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;
        }

        protected virtual void Start()
        {
            _bus.Subscribe(this);

            this.Group = this.Groups.GetById(NetScopeConstants.PeerScopeId);

            this.State = Enums.PeerState.Started;
        }

        public void Flush()
        {
            this.Manager.PollEvents();

            _bus.Flush();
        }

        private void HandleNetworkReceiveEvent(NetPeer sender, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            while (!reader.EndOfData)
            {
                this.Messages.Read(sender, reader, channel, deliveryMethod).Enqueue();
            }
        }

        void IPeer.Send(INetOutgoingMessage message)
        {
            this.Send(message);
        }

        protected virtual void Send(INetOutgoingMessage message)
        {
            foreach (NetPeer recipient in message.Recipients)
            {
                recipient.Send(message.Writer, message.OutgoingChannel, message.DeliveryMethod);
            }
        }

        protected abstract INetGroup GroupFactory(byte id);
    }
}
