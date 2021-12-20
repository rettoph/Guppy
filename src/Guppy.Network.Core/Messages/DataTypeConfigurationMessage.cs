using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Messages
{
    internal sealed class DataTypeConfigurationMessage
    {
        public UInt16 Id { get; internal set; }
        public String TypeAssemblyQualifiedName { get; internal set; }

        #region Read/Write Methods
        public static DataTypeConfigurationMessage Read(NetDataReader reader)
        {
            return new DataTypeConfigurationMessage()
            {
                Id = reader.GetUShort(),
                TypeAssemblyQualifiedName = reader.GetString()
            };
        }

        public static void Write(NetDataWriter writer, DataTypeConfigurationMessage dto)
        {
            writer.Put(dto.Id);
            writer.Put(dto.TypeAssemblyQualifiedName);
        }
        #endregion
    }
}
