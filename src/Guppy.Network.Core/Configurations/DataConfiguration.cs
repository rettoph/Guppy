using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public abstract class DataConfiguration : DynamicIdConfiguration
    {
        public readonly Type DataType;

        public readonly Action<NetDataWriter, IData> DataWriter;

        public readonly Func<NetDataReader, IData> DataReader;

        internal DataConfiguration(
            UInt16 id,
            Byte[] idBytes,
            Type type,
            Action<NetDataWriter, IData> writer,
            Func<NetDataReader, IData> reader) : base(id, idBytes)
        {
            this.DataType = type;
            this.DataWriter = writer;
            this.DataReader = reader;
        }
    }
}
