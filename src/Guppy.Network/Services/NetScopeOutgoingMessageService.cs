using Guppy.Network.Providers;
using Minnow.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    internal sealed class NetScopeOutgoingMessageService : INetScopeOutgoingMessageService
    {
        private readonly NetScope _scope;
        private readonly INetMessengerProvider _messengers;
        private readonly DoubleDictionary<int, Type, ConcurrentQueue<NetOutgoingMessage>> _outgoingQueues;

        public NetScopeOutgoingMessageService(
            NetScope scope,
            INetMessengerProvider messengers)
        {
            _scope = scope;
            _messengers = messengers;
            _outgoingQueues = _messengers.OrderBy(x => x.OutgoingPriority)
                 .ToDoubleDictionary(
                     keySelector1: x => x.OutgoingPriority,
                     keySelector2: x => x.Type,
                     elementSelector: x => new ConcurrentQueue<NetOutgoingMessage>());
        }

        public NetOutgoingMessage<T> Create<T>(INetTarget target, in T content)
        {
            return _messengers.CreateOutgoing<T>(_scope, target, in content);
        }

        public void Enqueue(NetOutgoingMessage message)
        {
            _outgoingQueues[message.Messenger.Type].Enqueue(message);
        }

        public void Send(int maximum)
        {
            int count = 0;

            foreach (ConcurrentQueue<NetOutgoingMessage> queue in _outgoingQueues.Values)
            {
                while (count++ < maximum && queue.TryDequeue(out NetOutgoingMessage? message))
                {
                    message.Send();
                    message.Recycle();
                }
            }
        }
    }
}
