using Autofac;
using Guppy.Common.Autofac;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Identity.Providers;
using LiteNetLib;
using System.Collections.ObjectModel;

namespace Guppy.Network.Peers
{
    public abstract class Peer : IDisposable
    {
        private readonly IDictionary<byte, NetScope> _scopes;

        public readonly EventBasedNetListener Listener;
        public readonly NetManager Manager;
        public readonly NetScope NetScope;

        public IUserProvider Users { get; }

        public abstract PeerType Type { get; }

        public readonly IReadOnlyDictionary<byte, NetScope> Scopes;

        public Peer(ILifetimeScope scope)
        {
            _scopes = new Dictionary<byte, NetScope>();

            this.Listener = new EventBasedNetListener();
            this.Manager = new NetManager(this.Listener);
            this.NetScope = scope.BeginLifetimeScope(LifetimeScopeTags.GuppyScope).Resolve<NetScope>();
            this.Users = new UserProvider();
            this.Scopes = new ReadOnlyDictionary<byte, NetScope>(_scopes);

            this.Listener.NetworkReceiveEvent += this.HandleNetworkReceiveEvent;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Listener.NetworkReceiveEvent -= this.HandleNetworkReceiveEvent;
        }

        protected virtual void Start()
        {
            this.Bind(this.NetScope, NetScopeConstants.PeerScopeId);
        }

        public void Bind(NetScope scope, byte id)
        {
            if (scope.Bound)
            {
                throw new InvalidOperationException($"{nameof(Peers.Peer)}::{nameof(Bind)} - {nameof(Network.NetScope)} has already been bound to a {nameof(Peers.Peer)} instance.");
            }

            if (!_scopes.TryAdd(id, scope))
            {
                throw new InvalidOperationException($"{nameof(Peers.Peer)}::{nameof(Bind)} - Another {nameof(Network.NetScope)} has already been bound to id '{id}'.");
            }

            scope.BindTo(this, id);
        }

        public void Unbind(NetScope scope)
        {
            if (!scope.Bound)
            {
                throw new InvalidOperationException($"{nameof(Peers.Peer)}::{nameof(Unbind)} - {nameof(Network.NetScope)} is not bound to any {nameof(Peers.Peer)}.");
            }

            if (scope.Peer != this)
            {
                throw new InvalidOperationException($"{nameof(Peers.Peer)}::{nameof(Unbind)} - {nameof(Network.NetScope)} is not bound to the current {nameof(Peers.Peer)}.");
            }

            if (!_scopes.Remove(scope.Id))
            {
                throw new InvalidOperationException($"{nameof(Peers.Peer)}::{nameof(Unbind)} - Unable to unbind {nameof(Network.NetScope)} from {nameof(Peers.Peer)}.");
            }

            scope.Unbind();
        }

        public void Unbind(byte id)
        {
            _scopes.Remove(id);
        }

        public void Flush()
        {
            this.Manager.PollEvents();

            this.NetScope.Bus.Flush();
        }

        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            while (!reader.EndOfData)
            {
                byte scopeId = reader.GetByte();
                NetScope scope = _scopes[scopeId];

                scope.Messages.Read(peer, reader, channel, deliveryMethod).Enqueue();
            }
        }
    }
}
