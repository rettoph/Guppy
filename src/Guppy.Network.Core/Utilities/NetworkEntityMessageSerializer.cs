using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    internal static class NetworkEntityMessageSerializer<TNetworkEntityMessage>
        where TNetworkEntityMessage : NetworkEntityMessage, new()
    {
        public static Func<NetDataReader, NetworkProvider, TNetworkEntityMessage> GetReader()
        {
            return Reader;
        }

        private static TNetworkEntityMessage Reader(NetDataReader reader, NetworkProvider network)
        {
            UInt16 networkId = reader.GetUShort();
            UInt32 serviceConfigurationId = reader.GetUInt();

            
            TNetworkEntityMessage message = new TNetworkEntityMessage()
            {
                NetworkId = networkId,
                ServiceConfigurationId = serviceConfigurationId
            };

            Int32 packetCount = reader.GetInt();
            for (Int32 i=0; i<packetCount; i++)
            {
                message.Packets.Add(network.ReadData<IPacket>(reader));
            }

            return message;
        }

        public static Action<NetDataWriter, NetworkProvider, TNetworkEntityMessage> GetWriter()
        {
            return Writer;
        }

        private static void Writer(NetDataWriter writer, NetworkProvider network, TNetworkEntityMessage message)
        {
            writer.Put(message.NetworkId);
            writer.Put(message.ServiceConfigurationId);

            writer.Put(message.Packets.Count());
            foreach(IPacket packet in message.Packets)
            {
                network.WriteData(writer, packet);
            }
        }
    }
}
