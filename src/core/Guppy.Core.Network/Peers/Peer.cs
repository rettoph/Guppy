using Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Definitions;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Identity.Services;
using Guppy.Core.Network.Common.Services;
using Guppy.Engine.Common.Autofac;
using LiteNetLib;

namespace Guppy.Core.Network.Common.Peers
{
    internal abstract class Peer : IDisposable, IPeer, IBaseSubscriber<IMessage>
    {
        private readonly IBus _bus;

        public readonly EventBasedNetListener Listener;
        public readonly NetManager Manager;
        public abstract PeerType Type { get; }
        public PeerState State { get; private set; }
        public UserService Users { get; }

        public INetMessageService Messages { get; }
        public INetGroupService Groups { get; }
        public INetGroup Group { get; private set; }

        IUserService IPeer.Users => this.Users;

        public Peer(ILifetimeScope scope, INetSerializerService serializers, IEnumerable<NetMessageTypeDefinition> messages)
        {
            ILifetimeScope innerScope = scope.BeginLifetimeScope(LifetimeScopeTags.GuppyScope);
            _bus = innerScope.Resolve<IBus>();

            this.Listener = new EventBasedNetListener();
            this.Manager = new NetManager(this.Listener);
            this.Users = new UserService();
            this.Messages = new NetMessageService(this, serializers, messages);
            this.Groups = new NetGroupService(this.GroupFactory);
            this.Group = null!;
            this.State = PeerState.NotStarted;

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
            this.Group.Add(_bus);

            this.State = PeerState.Started;
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
