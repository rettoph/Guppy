using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Identity.Services;
using System.Collections.ObjectModel;

namespace Guppy.Core.Network.Groups
{
    internal abstract class BaseNetGroup :
        INetGroup,
        IDisposable
    {
        private readonly List<INetScope> _scopes;
        private readonly List<IBus> _relays;

        public byte Id { get; private set; }
        public IPeer Peer { get; private set; }
        public INetScopeUserService Users { get; }
        public IReadOnlyList<INetScope> Scopes { get; private set; }
        public IReadOnlyList<IBus> Relays { get; private set; }

        IReadOnlyList<INetScope> INetGroup.Scopes => this.Scopes;
        IReadOnlyList<IBus> INetGroup.Relays => this.Relays;

        public BaseNetGroup(byte id, IPeer peer)
        {
            _scopes = [];
            _relays = [];

            this.Id = id;
            this.Peer = peer;
            this.Users = new NetScopeUserService();
            this.Scopes = new ReadOnlyCollection<INetScope>(_scopes);
            this.Relays = new ReadOnlyCollection<IBus>(_relays);
        }

        public virtual void Dispose()
        {
            this.Users.Dispose();
        }

        public INetOutgoingMessage<T> CreateMessage<T>(in T body)
            where T : notnull
        {
            return this.Peer.Messages.Create(this, body);
        }

        public void Publish(INetIncomingMessage im)
        {
            foreach (IBus relay in _relays)
            {
                relay.Enqueue(im);
            }
        }

        internal void Add(INetScope scope, IBus relay)
        {
            _scopes.Add(scope);
            _relays.Add(relay);
        }

        internal void Remove(INetScope scope, IBus relay)
        {
            _scopes.Remove(scope);
            _relays.Remove(relay);
        }

        public void Add(IBus relay)
        {
            _relays.Add(relay);
        }

        public void Remove(IBus relay)
        {
            _relays.Remove(relay);
        }
    }
}
