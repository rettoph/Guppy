using Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Contexts;
using Guppy.Core.Network.Groups;

namespace Guppy.Core.Network
{
    internal sealed class NetScope<T> : INetScope<T>,
        IDisposable,
        ISubscriber<INetOutgoingMessage>
    {
        private readonly IBus _bus;

        private BaseNetGroup Group { get; }

        INetGroup INetScope.Group => this.Group;

        public NetScope(IBus bus, ILifetimeScope scope, IEnumerable<NetScopeContext<T>> context)
        {
            this._bus = bus;

            this.Group = context.Single().GetGroup<BaseNetGroup>(scope);
            this.Group.Add(this, this._bus);

            this._bus.Subscribe(this);
        }

        public void Dispose()
        {
            this._bus.Unsubscribe(this);
            this.Group.Remove(this, this._bus);
        }

        public INetOutgoingMessage<TBody> CreateMessage<TBody>(in TBody body)
            where TBody : notnull
        {
            INetOutgoingMessage<TBody> om = this.Group.Peer.Messages.Create(this.Group, body);
            this.Enqueue(om);

            return om;
        }

        public void Enqueue(INetIncomingMessage message)
        {
            this._bus.Enqueue(message);
        }

        public void Enqueue(INetOutgoingMessage message)
        {
            this._bus.Enqueue(message);
        }

        public void Process(in Guid messageId, INetOutgoingMessage message)
        {
            message.Send();
        }
    }
}