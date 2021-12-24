using Guppy.Network;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Structs;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteNetLib.Utils
{
    public static class NetDataReaderExtensions
    {
        public static Boolean GetIf(this NetDataReader reader)
        {
            return reader.GetBool();
        }

        public static TEnum GetEnum<TEnum>(this NetDataReader reader)
            where TEnum : Enum
        {
            var byteVal = reader.GetByte();
            return EnumHelper.GetValues<TEnum>().First(v => Convert.ToByte(v) == byteVal);
        }

        public static Vector2 GetVector2(this NetDataReader reader)
        {
            return new Vector2(reader.GetFloat(), reader.GetFloat());
        }

        internal static void GetPackets<TNetworkEntityMessage>(this NetDataReader reader, NetworkProvider network, TNetworkEntityMessage message)
            where TNetworkEntityMessage : NetworkEntityMessage, new()
        {
            Int32 packetCount = reader.GetInt();
            for (Int32 i = 0; i < packetCount; i++)
            {
                message.Packets.Add(network.ReadData<IPacket>(reader));
            }
        }
    }
}
