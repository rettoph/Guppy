using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    public sealed class NetworkEntityMessageService : Publisher<IData>
    {
        private delegate IData PacketFactoryDelegate();

        #region Private Fields
        private ServiceProvider _provider;
        private Dictionary<Type, PacketFactoryDelegate> _packetFactories;
        private Dictionary<Type, NetworkEntityMessagePinger> _messagePingers;
        #endregion

        #region Internal Fields
        internal IMagicNetworkEntity entity { get; set; }
        #endregion

        #region Lifecyele Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;
            _packetFactories = new Dictionary<Type, PacketFactoryDelegate>();
            _messagePingers = new Dictionary<Type, NetworkEntityMessagePinger>();
        }

        protected override void Uninitialize()
        {
            base.Uninitialize();

            _packetFactories.Clear();

            foreach(NetworkEntityMessagePinger pinger in _messagePingers.Values)
            {
                pinger.Dispose();
            }

            _messagePingers.Clear();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Register a new message to be broadcasted at an interval.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="factory">a factory for the message type.</param>
        /// <param name="minimumInterval">The minimum amount of time that may pass before broadcasting the message.</param>
        /// <param name="maximumInterval">The maximum amount of time that may pass before broadcasting the message. If this threshold is crossed, the message will be sent.</param>
        /// <param name="enabled">Indicates whether or not the pinger should start enabled.</param>
        /// <returns></returns>
        public NetworkEntityMessagePinger RegisterPinger<TMessage>(
            Func<TMessage> factory, 
            Double minimumInterval, 
            Double maximumInterval, 
            Boolean enabled = true)
                where TMessage : NetworkEntityMessage<TMessage>
        {
            void Sender()
            {
                if(this.entity.Status != ServiceStatus.Ready)
                {
                    return;
                }

                this.entity.SendMessage<TMessage>(factory());
            };

            NetworkEntityMessagePinger pinger = _provider.GetService<NetworkEntityMessagePinger>((p, _, _) =>
            {
                p.sender = Sender;
                p.MinimumInterval = minimumInterval;
                p.MaximumInterval = maximumInterval;
                p.Enabled = enabled;
            });

            _messagePingers.Add(typeof(TMessage), pinger);

            return pinger;
        }

        /// <summary>
        /// Register a new message to be broadcasted at an interval.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="factory">a factory for the message type.</param>
        /// <param name="minimumInterval">The minimum amount of time that may pass before broadcasting the message.</param>
        /// <param name="maximumInterval">The maximum amount of time that may pass before broadcasting the message. If this threshold is crossed, the message will be sent.</param>
        /// <param name="enabled">Indicates whether or not the pinger should start enabled.</param>
        /// <returns></returns>
        public NetworkEntityMessagePinger RegisterPinger<TMessage>(
            IDataFactory<TMessage> factory,
            Double minimumInterval,
            Double maximumInterval,
            Boolean enabled = true)
                where TMessage : NetworkEntityMessage<TMessage>
        {
            return this.RegisterPinger<TMessage>(
                factory.Create,
                minimumInterval,
                maximumInterval,
                enabled);
        }

        /// <summary>
        /// Register a new message to be broadcasted at an interval.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="minimumInterval">The minimum amount of time that may pass before broadcasting the message.</param>
        /// <param name="maximumInterval">The maximum amount of time that may pass before broadcasting the message. If this threshold is crossed, the message will be sent.</param>
        /// <param name="enabled">Indicates whether or not the pinger should start enabled.</param>
        /// <returns></returns>
        public NetworkEntityMessagePinger RegisterPinger<TMessage>(
            Double minimumInterval,
            Double maximumInterval,
            Boolean enabled = true)
                where TMessage : NetworkEntityMessage<TMessage>, new()
        {
            return this.RegisterPinger<TMessage>(
                () => new TMessage(),
                minimumInterval,
                maximumInterval,
                enabled);
        }
        public void DeregisterPinger<TMessage>()
            where TMessage : NetworkEntityMessage<TMessage>
        {
            _messagePingers.Remove(typeof(TMessage));
        }

        public NetworkEntityMessagePinger GetPinger<TMessage>()
            where TMessage : NetworkEntityMessage<TMessage>
        {
            return _messagePingers[typeof(TMessage)];
        }

        public void RegisterPacket<TPacket, TNetworkEntityMessage>(IDataFactory<TPacket> factory)
            where TPacket : class, IData
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            if(_packetFactories.ContainsKey(typeof(TNetworkEntityMessage)))
            {
                _packetFactories[typeof(TNetworkEntityMessage)] += factory.Create;
                return;
            }

            _packetFactories.Add(typeof(TNetworkEntityMessage), factory.Create);
        }
        public void DeregisterPacket<TPacket, TNetworkEntityMessage>(IDataFactory<TPacket> factory)
            where TPacket : class, IData
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            _packetFactories[typeof(TNetworkEntityMessage)] -= factory.Create;
        }

        /// <summary>
        /// Create a new list of all packets registered to the input 
        /// NetworkEntityMessage type.
        /// </summary>
        /// <typeparam name="TNetworkEntityMessage"></typeparam>
        /// <returns></returns>
        public IEnumerable<IData> GetAll<TNetworkEntityMessage>()
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            if(_packetFactories.TryGetValue(typeof(TNetworkEntityMessage), out PacketFactoryDelegate factories))
            {
                foreach (PacketFactoryDelegate factory in factories.GetInvocationList())
                {
                    yield return factory();
                }
            }
        }

        public void Process(NetworkEntityMessage message)
        {
            this.Publish(message);

            foreach(IData packet in message.Packets)
            {
                this.Publish(packet);
            }
        }
        #endregion
    }
}
