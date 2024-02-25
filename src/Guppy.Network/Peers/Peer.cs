using Autofac;
using Guppy.Common.Autofac;
using Guppy.Messaging;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Identity.Services;
using LiteNetLib;
using System.Collections.ObjectModel;

namespace Guppy.Network.Peers
{
    internal abstract class Peer : IDisposable, IPeer
    {
        private readonly IDictionary<byte, INetScope> _scopes;

        public readonly EventBasedNetListener Listener;
        public readonly NetManager Manager;
        public readonly INetScope NetScope;
        public readonly IBus NetScopeBus;

        public IUserService Users { get; }
        public abstract PeerType Type { get; }
        public IReadOnlyDictionary<byte, INetScope> Scopes { get; }

        public Peer(ILifetimeScope scope)
        {
            _scopes = new Dictionary<byte, INetScope>();

            ILifetimeScope innerScope = scope.BeginLifetimeScope(LifetimeScopeTags.GuppyScope);
            this.NetScope = innerScope.Resolve<INetScope>();
            this.NetScopeBus = innerScope.Resolve<IBus>();

            this.Listener = new EventBasedNetListener();
            this.Manager = new NetManager(this.Listener);
            this.Users = new UserService();
            this.Scopes = new ReadOnlyDictionary<byte, INetScope>(_scopes);

            this.Listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;
        }

        protected virtual void Start()
        {
            this.NetScope.AttachPeer(this, NetScopeConstants.PeerScopeId);
        }

        bool IPeer.TryAttachNetScope(INetScope scope, byte id)
        {
            if (!_scopes.TryAdd(id, scope))
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(IPeer.TryAttachNetScope)} - Another {nameof(Network.NetScope)} has already been bound to id '{id}'.");
            }

            return true;
        }

        void IPeer.DetachNetScope(INetScope scope)
        {
            if (scope.Peer != this)
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(Unbind)} - {nameof(Network.NetScope)} is not bound to the current {nameof(Peer)}.");
            }

            if (!_scopes.Remove(scope.Id))
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(Unbind)} - Unable to unbind {nameof(Network.NetScope)} from {nameof(Peer)}.");
            }
        }

        public void Unbind(byte id)
        {
            _scopes.Remove(id);
        }

        public void Flush()
        {
            this.Manager.PollEvents();

            this.NetScopeBus.Flush();
        }

        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            while (!reader.EndOfData)
            {
                byte scopeId = reader.GetByte();
                INetScope scope = _scopes[scopeId];

                scope.Messages.Read(peer, reader, channel, deliveryMethod).Enqueue();
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
    }
}
