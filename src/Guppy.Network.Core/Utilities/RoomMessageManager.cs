using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Threading.Helpers;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Minnow.General;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Contains a <see cref="Room"/>'s bound scope's active messages
    /// and helpful accessors for internal message configuration/processing.
    /// </summary>
    public sealed class RoomMessageManager : IDisposable
    {
        private Room _room;
        private ServiceProvider _scope;
        private MessageBus _incomingBus;
        private NetworkProvider _network;

        private ConcurrentQueue<OutgoingMessage> _outgoingMessagePool;
        private Dictionary<Int32, ConcurrentQueue<OutgoingMessage>> _outgoingMessages;

        private CancellationTokenSource _cancellation;

        internal RoomMessageManager(Room room, NetworkProvider network, ServiceProvider scope)
        {
            _room = room;
            _scope = scope;
            _network = network;

            this.ConfigureOutgoing();
            this.ConfigureIncoming();

            _cancellation = new CancellationTokenSource();
            TaskHelper.CreateLoop(this.FlushOutgoingMessages, 16, _cancellation.Token);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare all outgoing message configurations.
        /// </summary>
        private void ConfigureOutgoing()
        {
            // Load all messages that may be sent by the current peer
            var authorization = _scope.Settings.Get<NetworkAuthorization>();
            var configurations = _network.MessageConfigurations.Where(c => c.CanSend(authorization));

            _outgoingMessagePool = new ConcurrentQueue<OutgoingMessage>();

            _outgoingMessages = configurations.DistinctBy(configuration => configuration.OutgoingPriority)
                .OrderBy(configuration => configuration.OutgoingPriority)
                .ToDictionary(
                    keySelector: configuration => configuration.OutgoingPriority,
                    elementSelector: configuration => new ConcurrentQueue<OutgoingMessage>());
        }

        /// <summary>
        /// Prepare all incoming message configurations.
        /// </summary>
        private void ConfigureIncoming()
        {
            // Load all messages that may be sent by the alternative authorization
            var authorization = _scope.Settings.Get<NetworkAuthorization>();
            var configurations = _network.MessageConfigurations.Where(c => c.CanRecieve(authorization));

            // Generate a lookup table of valid incoming message configuration bus queues and their types...
            Dictionary<Int32, Type[]> incomingPriorities = configurations.GroupBy(configuration => configuration.IncomingPriority)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => g.Select(configuration => configuration.Type).ToArray());

            // Register all incoming message handlers to the primary scope bus
            // Note, outgoing messages have their own bus. OutgoingMessageBus
            _scope.Service(out _incomingBus);
            foreach ((Int32 incomingPriority, Type[] types) in incomingPriorities)
            {
                _incomingBus.GetQueue(incomingPriority).RegisterTypes(types);
            }

            // Register all message processors into the bus
            foreach (NetworkMessageConfiguration configuration in configurations)
            {
                configuration.TryRegisterIncomingProcessor(_scope, _incomingBus);
            }
        }

        /// <summary>
        /// Send a message within the current room to a specific recipient
        /// </summary>
        /// <param name="data"></param>
        public void Send<TData>(TData data, NetPeer reciepient)
            where TData : IData
        {
            this.CreateOutgoingMessage().Prepare(data, reciepient);
        }

        /// <summary>
        /// Send a message within the current room to a specific collection of recipients
        /// </summary>
        /// <param name="data"></param>
        public void Send<TData>(TData data, IEnumerable<NetPeer> reciepients)
            where TData : IData
        {
            this.CreateOutgoingMessage().Prepare(data, reciepients);
        }

        /// <summary>
        /// Send a message within the current room to all joined peers
        /// </summary>
        /// <param name="data"></param>
        public void Send<TData>(TData data)
            where TData : IData
        {
            this.CreateOutgoingMessage().Prepare(data, _room.Users.NetPeers);
        }

        /// <summary>
        /// Send a message within the current room to a specific recipient
        /// </summary>
        /// <param name="data"></param>
        public void Send<TData>(ref TData data, NetPeer reciepient)
            where TData : struct, IData
        {
            this.CreateOutgoingMessage().Prepare(data, reciepient);
        }

        /// <summary>
        /// Send a message within the current room to a specific collection of recipients
        /// </summary>
        /// <param name="data"></param>
        public void Send<TData>(ref TData data, IEnumerable<NetPeer> reciepients)
            where TData : struct, IData
        {
            this.CreateOutgoingMessage().Prepare(data, reciepients);
        }

        /// <summary>
        /// Send a message within the current room to all joined peers
        /// </summary>
        /// <param name="data"></param>
        public void Send<TData>(ref TData data)
            where TData : struct, IData
        {
            this.CreateOutgoingMessage().Prepare(data, _room.Users.NetPeers);
        }

        /// <summary>
        /// Process an incoming message
        /// </summary>
        /// <param name="message"></param>
        internal void TryEnqueueIncomingMessage(NetworkMessage message)
        {
            _incomingBus.Enqueue(message.Data);
        }

        internal void TryEnqueueOutgoingMessage(OutgoingMessage message)
        {
            if(message.Priority == -1000)
            {

            }
            _outgoingMessages[message.Priority].Enqueue(message);
        }

        private OutgoingMessage CreateOutgoingMessage()
        {
            if(_outgoingMessagePool.TryDequeue(out OutgoingMessage message))
            {
                return message;
            }

            return new OutgoingMessage(_room, _network);
        }

        private void FlushOutgoingMessages(GameTime obj)
        {
            Int32 sent = 0;
            Int32 total = (Int32)(obj.ElapsedGameTime.TotalSeconds * _network.OutgoingRateLimit) + 1;

            foreach(ConcurrentQueue<OutgoingMessage> queue in _outgoingMessages.Values)
            {
                while (sent < total && queue.TryDequeue(out OutgoingMessage outgoing))
                {
                    outgoing.Send(ref sent);
                }
            }
        }
    }
}
