using Guppy.Network.Messages;
using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Threading.Interfaces;

namespace Guppy.Network.Configurations
{
    public class DataConfiguration
    {
        public readonly DynamicId Id;

        public readonly Type Type;

        public readonly Action<NetDataWriter, NetworkProvider, IData> Writer;

        public readonly Func<NetDataReader, NetworkProvider, IData> Reader;

        internal DataConfiguration(
            DynamicId id,
            Type type,
            Action<NetDataWriter, NetworkProvider, IData> writer,
            Func<NetDataReader, NetworkProvider, IData> reader)
        {
            this.Id = id;
            this.Type = type;
            this.Writer = writer;
            this.Reader = reader;
        }
    }
}
