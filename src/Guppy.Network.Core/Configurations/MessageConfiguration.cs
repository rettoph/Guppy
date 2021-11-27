using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public class MessageConfiguration : DataConfiguration
    {
        public readonly String Name;

        public MessageConfiguration(
            UInt16 id, 
            Byte[] idBytes, 
            String name,
            Type type, 
            Action<NetDataWriter, IData> writer, 
            Func<NetDataReader, IData> reader) : base(id, idBytes, type, writer, reader)
        {
            this.Name = name;
        }
    }
}
