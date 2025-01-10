using System.Collections.ObjectModel;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Identity.Services;

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
            this._scopes = [];
            this._relays = [];

            this.Id = id;
            this.Peer = peer;
            this.Users = new NetScopeUserService();
            this.Scopes = new ReadOnlyCollection<INetScope>(this._scopes);
            this.Relays = new ReadOnlyCollection<IBus>(this._relays);
        }

        public virtual void Dispose() => this.Users.Dispose();

        public INetOutgoingMessage<T> CreateMessage<T>(in T body)
            where T : notnull => this.Peer.Messages.Create(this, body);

        public void Publish(INetIncomingMessage im)
        {
            foreach (IBus relay in this._relays)
            {
                relay.Enqueue(im);
            }
        }

        internal void Add(INetScope scope, IBus relay)
        {
            this._scopes.Add(scope);
            this._relays.Add(relay);
        }

        internal void Remove(INetScope scope, IBus relay)
        {
            this._scopes.Remove(scope);
            this._relays.Remove(relay);
        }

        public void Add(IBus relay) => this._relays.Add(relay);

        public void Remove(IBus relay) => this._relays.Remove(relay);
    }
}