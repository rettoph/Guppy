
using Guppy.Messaging;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using System.Collections.ObjectModel;

namespace Guppy.Network
{
    internal sealed class NetScope : INetScope, ISubscriber<INetOutgoingMessage>, ISubscriber<INetIncomingMessage<UserAction>>
    {
        private readonly IBus _bus;
        private readonly List<INetGroup> _groups;

        public IReadOnlyList<INetGroup> Groups { get; }

        public PeerType Type { get; private set; }

        public NetScope(IBus bus)
        {
            _bus = bus;
            _groups = new List<INetGroup>();

            this.Groups = new ReadOnlyCollection<INetGroup>(_groups);

            _bus.Subscribe(this);
        }

        public void Enqueue(INetIncomingMessage message)
        {
            _bus.Enqueue(message);
        }

        public void Enqueue(INetOutgoingMessage message)
        {
            _bus.Enqueue(message);
        }

        void INetScope.Add(INetGroup group)
        {
            _groups.Add(group);
            this.Type |= group.Peer.Type;
        }

        void INetScope.Remove(INetGroup group)
        {
            _groups.Remove(group);
            this.Type = _groups.Select(x => x.Peer.Type).Aggregate((t1, t2) => t1 | t2);
        }

        public void Process(in Guid messageId, INetOutgoingMessage message)
        {
            message.Send();
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
        {
            message.Group.Process(message);
        }
    }
}
