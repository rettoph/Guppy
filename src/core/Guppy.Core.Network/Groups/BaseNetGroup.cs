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
        private readonly List<IMessageBus> _messageBusses;

        public byte Id { get; private set; }
        public IPeer Peer { get; private set; }
        public INetScopeUserService Users { get; }
        public IReadOnlyList<INetScope> Scopes { get; private set; }
        public IReadOnlyList<IMessageBus> Relays { get; private set; }

        IReadOnlyList<INetScope> INetGroup.Scopes => this.Scopes;
        IReadOnlyList<IMessageBus> INetGroup.Relays => this.Relays;

        public BaseNetGroup(byte id, IPeer peer)
        {
            this._scopes = [];
            this._messageBusses = [];

            this.Id = id;
            this.Peer = peer;
            this.Users = new NetScopeUserService();
            this.Scopes = new ReadOnlyCollection<INetScope>(this._scopes);
            this.Relays = new ReadOnlyCollection<IMessageBus>(this._messageBusses);
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

        public void Publish<T>(T im)
            where T : INetIncomingMessage
        {
            foreach (IMessageBus messageBus in this._messageBusses)
            {
                messageBus.Enqueue(im);
            }
        }

        internal void Add(INetScope scope, IMessageBus relay)
        {
            this._scopes.Add(scope);
            this._messageBusses.Add(relay);
        }

        internal void Remove(INetScope scope, IMessageBus relay)
        {
            this._scopes.Remove(scope);
            this._messageBusses.Remove(relay);
        }

        public void Add(IMessageBus relay)
        {
            this._messageBusses.Add(relay);
        }

        public void Remove(IMessageBus relay)
        {
            this._messageBusses.Remove(relay);
        }
    }
}