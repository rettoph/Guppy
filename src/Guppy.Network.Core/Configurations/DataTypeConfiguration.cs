using Guppy.Network.Dtos;
using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public class DataTypeConfiguration
    {
        public readonly DynamicId Id;

        public readonly Type Type;

        public readonly Action<NetDataWriter, IData> Writer;

        public readonly Func<NetDataReader, IData> Reader;

        internal DataTypeConfiguration(
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

        internal DataTypeConfigurationDto ToDto()
        {
            return new DataTypeConfigurationDto()
            {
                Id = this.Id.Value,
                TypeAssemblyQualifiedName = this.Type.AssemblyQualifiedName
            };
        }
    }
}
