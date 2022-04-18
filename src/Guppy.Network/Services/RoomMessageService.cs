using Guppy.Network.Providers;
using Guppy.Threading;
using LiteNetLib;
using Minnow.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public sealed class RoomMessageService : MessageService
    {
        private INetMessengerProvider _messengers;
        private Room _room;
        private Bus? _bus;

        private DoubleDictionary<int, Type, ConcurrentQueue<NetOutgoingMessage>> _outgoingQueues;

        public RoomMessageService(
            INetMessengerProvider messengers, 
            Room room)
        {
            _messengers = messengers;
            _room = room;

            _outgoingQueues = _messengers.OrderBy(x => x.OutgoingPriority)
                .ToDoubleDictionary(
                    keySelector1: x => x.OutgoingPriority,
                    keySelector2: x => x.Type,
                    elementSelector: x => new ConcurrentQueue<NetOutgoingMessage>());
        }

        public override void Dispose()
        {
            foreach (ConcurrentQueue<NetOutgoingMessage> queue in _outgoingQueues.Values)
            {
                while (queue.TryDequeue(out NetOutgoingMessage? message))
                {
                    message.Recycle();
                }
            }
        }

        /// <summary>
        /// Create a new outgoing message configured for the current room.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public override NetOutgoingMessage<T> CreateOutgoing<T>(in T message)
        {
            return _messengers.CreateOutgoing<T>(_room, in message);
        }

        /// <summary>
        /// Process the incoming message.
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessIncoming(NetIncomingMessage message)
        {
            _bus!.Enqueue(message);
        }

        /// <summary>
        /// Enqueue an outgoing message to be sent.
        /// </summary>
        /// <param name="outgoing"></param>
        public void EnqueueOutgoing(NetOutgoingMessage outgoing)
        {
            _outgoingQueues[outgoing.Messenger.Type].Enqueue(outgoing);
        }

        /// <summary>
        /// Send all enqueued outgoing messages, up to the given maximum.
        /// </summary>
        /// <param name="maximum"></param>
        public void SendEnqueued(int maximum)
        {
            int count = 0;

            foreach(ConcurrentQueue<NetOutgoingMessage> queue in _outgoingQueues.Values)
            {
                while(count++ < maximum && queue.TryDequeue(out NetOutgoingMessage? message))
                {
                    message.Send();
                    message.Recycle();
                }
            }
        }

        /// <summary>
        /// Designate <paramref name="bus"/> as the target location
        /// all incoming messages are to be published to.
        /// </summary>
        /// <param name="bus"></param>
        public void AttachBus(Bus bus)
        {
            _bus = bus;
        }
    }
}
