using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Dtos
{
    internal sealed class MessageConfigurationDto
    {
        public UInt16 Id { get; internal set; }
        public String Name { get; internal set; }
        public UInt16 DataTypeId { get; internal set; }

        #region Read/Write Methods
        public static MessageConfigurationDto Read(NetDataReader reader)
        {
            return new MessageConfigurationDto()
            {
                Id = reader.GetUShort(),
                Name = reader.GetString(),
                DataTypeId = reader.GetUShort()
            };
        }

        public static void Write(NetDataWriter writer, MessageConfigurationDto dto)
        {
            writer.Put(dto.Id);
            writer.Put(dto.Name);
            writer.Put(dto.DataTypeId);
        }
        #endregion
    }
}
