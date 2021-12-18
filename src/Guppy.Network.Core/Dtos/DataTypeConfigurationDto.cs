using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Dtos
{
    internal sealed class DataTypeConfigurationDto
    {
        public UInt16 Id { get; internal set; }
        public String TypeAssemblyQualifiedName { get; internal set; }

        #region Read/Write Methods
        public static DataTypeConfigurationDto Read(NetDataReader reader)
        {
            return new DataTypeConfigurationDto()
            {
                Id = reader.GetUShort(),
                TypeAssemblyQualifiedName = reader.GetString()
            };
        }

        public static void Write(NetDataWriter writer, DataTypeConfigurationDto dto)
        {
            writer.Put(dto.Id);
            writer.Put(dto.TypeAssemblyQualifiedName);
        }
        #endregion
    }
}
