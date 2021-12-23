using Guppy.EntityComponent.DependencyInjection;
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
    public sealed class NetworkEntityPacketService : MessageQueue<IPacket>
    {
        private delegate IPacket PacketFactoryDelegate();

        #region Private Fields
        private Dictionary<Type, PacketFactoryDelegate> _packetFactories;
        #endregion

        #region Lifecyele Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _packetFactories = new Dictionary<Type, PacketFactoryDelegate>();
        }

        protected override void Release()
        {
            base.Release();

            _packetFactories.Clear();
        }
        #endregion

        #region Helper Methods
        public void RegisterPacket<TPacket, TNetworkEntityMessage>(IMessageFactory<TPacket> factory)
            where TPacket : class, IPacket
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            if(_packetFactories.TryGetValue(typeof(TNetworkEntityMessage), out PacketFactoryDelegate del))
            {
                del += factory.Create;
                return;
            }

            _packetFactories.Add(typeof(TNetworkEntityMessage), factory.Create);
        }
        public void DeregisterPacket<TPacket, TNetworkEntityMessage>(IMessageFactory<TPacket> factory)
            where TPacket : class, IPacket
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            _packetFactories[typeof(TNetworkEntityMessage)] -= factory.Create;
        }

        /// <summary>
        /// Create a new list of all packets registered to the input 
        /// NetworkEntityMessage type.
        /// </summary>
        /// <typeparam name="TNetworkEntityMessage"></typeparam>
        /// <returns></returns>
        public IEnumerable<IPacket> GetAll<TNetworkEntityMessage>()
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            if(_packetFactories.TryGetValue(typeof(TNetworkEntityMessage), out PacketFactoryDelegate factories))
            {
                foreach (PacketFactoryDelegate factory in factories.GetInvocationList())
                {
                    yield return factory();
                }
            }
        }
        #endregion
    }
}
