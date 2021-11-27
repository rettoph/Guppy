using DotNetUtils.General.Interfaces;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Guppy.Network.Configurations
{
    public class PacketConfiguration : DataConfiguration
    {
        public PacketConfiguration(
            UInt16 id, 
            Byte[] idBytes, 
            Type type, 
            Action<NetDataWriter, IData> writer, 
            Func<NetDataReader, IData> reader) : base(id, idBytes, type, writer, reader)
        {
        }
    }
}
