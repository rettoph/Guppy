using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    public sealed class CreateNetworkEntityMessage : NetworkEntityMessage<CreateNetworkEntityMessage>
    {
        public UInt32 ServiceConfigurationId { get; internal set; }

        #region Read/Write/Filter Methods
        internal static CreateNetworkEntityMessage Read(NetDataReader reader, NetworkProvider network)
        {
            UInt16 networkId = reader.GetUShort();
            UInt32 serviceConfigurationId = reader.GetUInt();

            CreateNetworkEntityMessage message = new CreateNetworkEntityMessage()
            {
                NetworkId = networkId,
                ServiceConfigurationId = serviceConfigurationId
            };

            reader.GetPackets(network, message);

            return message;
        }

        internal static void Write(NetDataWriter writer, NetworkProvider network, CreateNetworkEntityMessage message)
        {
            writer.Put(message.NetworkId);
            writer.Put(message.ServiceConfigurationId);

            writer.PutPackets(network, message);
        }

        /// <summary>
        /// Simple method used to determin whether or not 
        /// a CreateNetworkEntityMessage should be processed
        /// within the peer.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static bool Filter(ServiceProvider p, NetworkMessageConfiguration c)
        {
            if(p.GetService<Peer>() is not ClientPeer)
            {
                return false;
            }

            if(p.GetService<Scene>() is null)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
