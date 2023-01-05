using Guppy.Common;
using Guppy.Common.Collections;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Providers;
using Guppy.Resources;
using Guppy.Resources.Providers;
using LiteNetLib;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public abstract class Peer : IDisposable
    {
        private readonly IDictionary<byte, NetScope> _scopes;

        public readonly EventBasedNetListener Listener;
        public readonly NetManager Manager;
        public readonly NetScope Scope;

        public IUserProvider Users { get; }

        public abstract PeerType Type { get; }

        public readonly IReadOnlyDictionary<byte, NetScope> Scopes;

        public Peer(IScoped<NetScope> scope)
        {
            _scopes = new Dictionary<byte, NetScope>();

            this.Listener = new EventBasedNetListener();
            this.Manager = new NetManager(this.Listener);
            this.Scope = scope.Instance;
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
            this.Bind(this.Scope, NetScopeConstants.PeerScopeId);
            this.Scope.Bus.Initialize();
        }

        public void Bind(NetScope scope, byte id)
        {
            if(scope.Bound)
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(Bind)} - {nameof(NetScope)} has already been bound to a {nameof(Peer)} instance.");
            }

            if(!_scopes.TryAdd(id, scope))
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(Bind)} - Antoher {nameof(NetScope)} has already been bound to id '{id}'.");
            }

            scope.BindTo(this, id);
        }

        public void Unbind(NetScope scope)
        {
            if(!scope.Bound)
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(Unbind)} - {nameof(NetScope)} is not bound to any {nameof(Peer)}.");
            }

            if(scope.Peer != this)
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(Unbind)} - {nameof(NetScope)} is not bound to the current {nameof(Peer)}.");
            }

            if(!_scopes.Remove(scope.Id))
            {
                throw new InvalidOperationException($"{nameof(Peer)}::{nameof(Unbind)} - Unable to unbind {nameof(NetScope)} from {nameof(Peer)}.");
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

            this.Scope.Bus.Flush();
        }

        private void HandleNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            while(!reader.EndOfData)
            {
                byte scopeId = reader.GetByte();
                NetScope scope = _scopes[scopeId];

                scope.Messages.Read(peer, reader, channel, deliveryMethod).Enqueue();
            }
        }
    }
}
