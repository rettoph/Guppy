using Guppy.Network.Messages;
using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public class DataConfiguration
    {
        public readonly DynamicId Id;

        public readonly Type Type;

        public readonly Action<NetDataWriter, IData> Writer;

        public readonly Func<NetDataReader, IData> Reader;

        internal DataConfiguration(
            DynamicId id,
            Type type,
            Action<NetDataWriter, IData> writer,
            Func<NetDataReader, IData> reader)
        {
            this.Id = id;
            this.Type = type;
            this.Writer = writer;
            this.Reader = reader;
        }

        internal DataTypeConfigurationMessage GetMessage()
        {
            return new DataTypeConfigurationMessage()
            {
                Id = this.Id.Value,
                TypeAssemblyQualifiedName = this.Type.AssemblyQualifiedName
            };
        }
    }
}
