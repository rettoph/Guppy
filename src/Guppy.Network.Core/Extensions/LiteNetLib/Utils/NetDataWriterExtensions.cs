﻿using Guppy.Network;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Structs;
using Guppy.Threading.Interfaces;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteNetLib.Utils
{
    public static class NetDataWriterExtensions
    {
        public static Boolean PutIf(this NetDataWriter writer, Boolean value)
        {
            writer.Put(value);

            return value;
        }

        public static void Put<TEnum>(this NetDataWriter writer, TEnum value)
            where TEnum : Enum
        {
            writer.Put(Convert.ToByte(value));
        }

        public static void Put(this NetDataWriter writer, in Vector2 vector2)
        {
            writer.Put(vector2.X);
            writer.Put(vector2.Y);
        }
        public static void Put(this NetDataWriter writer, Vector2 vector2)
        {
            writer.Put(in vector2);
        }

        internal static void PutPackets<TNetworkEntityMessage>(this NetDataWriter writer, NetworkProvider network, TNetworkEntityMessage message)
            where TNetworkEntityMessage : NetworkEntityMessage
        {
            writer.Put(message.Packets.Count);
            foreach (IData packet in message.Packets)
            {
                network.WriteData(writer, packet);
            }
        }
    }
}
