
using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Groups;
using Guppy.Core.Network.Common.Messages;
using System.Collections.ObjectModel;

namespace Guppy.Core.Network.Common
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

        public void Add(INetGroup group)
        {
            _groups.Add(group);
            this.Type |= group.Peer.Type;
        }

        public void Remove(INetGroup group)
        {
            _groups.Remove(group);
            if (_groups.Count == 0)
            {
                this.Type = PeerType.None;
            }
            else
            {
                this.Type = _groups.Select(x => x.Peer.Type).Aggregate((t1, t2) => t1 | t2);
            }
        }

        public void Process(in Guid messageId, INetOutgoingMessage message)
        {
            message.Send();
        }

        public void Process(in Guid messsageId, INetIncomingMessage<UserAction> message)
        {
            if (message.Group is BaseNetGroup casted)
            {
                casted.Process(message);
            }
        }
    }
}
