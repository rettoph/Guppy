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

            
            TNetworkEntityMessage message = new TNetworkEntityMessage()
            {
                NetworkId = networkId
            };

            reader.GetPackets(network, message);

            return message;
        }

        public static Action<NetDataWriter, NetworkProvider, TNetworkEntityMessage> GetWriter()
        {
            return Writer;
        }

        private static void Writer(NetDataWriter writer, NetworkProvider network, TNetworkEntityMessage message)
        {
            writer.Put(message.NetworkId);

            writer.PutPackets(network, message);
        }
    }
}
