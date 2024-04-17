using Autofac;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common.Contexts;
using Guppy.Core.Network.Common.Groups;

namespace Guppy.Core.Network.Common
{
    internal sealed class NetScope<T> : INetScope<T>,
        IDisposable,
        ISubscriber<INetOutgoingMessage>
    {
        private readonly IBus _bus;

        private BaseNetGroup Group { get; }

        INetGroup INetScope.Group => this.Group;

        public NetScope(IBus bus, ILifetimeScope scope, NetScopeContext<T> context)
        {
            _bus = bus;

            this.Group = context.GetGroup<BaseNetGroup>(scope);
            this.Group.Add(this, _bus);

            _bus.Subscribe(this);
        }

        public void Dispose()
        {
            _bus.Unsubscribe(this);
            this.Group.Remove(this, _bus);
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
            _bus.Enqueue(message);
        }

        public void Enqueue(INetOutgoingMessage message)
        {
            _bus.Enqueue(message);
        }

        public void Process(in Guid messageId, INetOutgoingMessage message)
        {
            message.Send();
        }
    }
}
