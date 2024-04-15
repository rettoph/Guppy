using Guppy.Core.Network.Common.Identity.Services;
using Guppy.Core.Network.Common.Messages;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;

namespace Guppy.Core.Network.Common.Groups
{
    internal abstract class BaseNetGroup :
        INetGroup,
        IDisposable
    {
        public byte Id { get; private set; }
        public IPeer Peer { get; private set; }
        public INetScopeUserService Users { get; }
        public NetScope Scope { get; private set; }

        INetScope INetGroup.Scope => this.Scope;

        public BaseNetGroup(byte id, IPeer peer)
        {
            this.Id = id;
            this.Peer = peer;
            this.Users = new NetScopeUserService();
            this.Scope = null!;

            Attach(this.Peer.DefaultNetScope);
        }

        public virtual void Dispose()
        {
            this.Users.Dispose();
        }

        public void Attach(INetScope scope)
        {
            if (this.Scope is not null)
            {
                Detach();
            }

            if (scope is not NetScope casted)
            {
                throw new ArgumentException();
            }

            this.Scope = casted;
            this.Scope.Add(this);
        }

        public void Detach()
        {
            if (this.Scope is null)
            {
                throw new InvalidOperationException($"{nameof(BaseNetGroup)}::{nameof(Detach)} - {nameof(BaseNetGroup)} is not bound to an {nameof(INetScope)} instance.");
            }

            this.Scope.Remove(this);
            this.Scope = null!;
        }

        public INetOutgoingMessage<T> CreateMessage<T>(in T body) where T : notnull
        {
            return this.Peer.Messages.Create(this, body);
        }

        public virtual void Process(INetIncomingMessage<UserAction> message)
        {
        }
    }
}
