using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Messages
{
    internal sealed class MessageConfigurationMessage
    {
        public UInt16 Id { get; internal set; }
        public String Name { get; internal set; }
        public UInt16 DataTypeId { get; internal set; }

        #region Read/Write Methods
        public static MessageConfigurationMessage Read(NetDataReader reader)
        {
            return new MessageConfigurationMessage()
            {
                Id = reader.GetUShort(),
                Name = reader.GetString(),
                DataTypeId = reader.GetUShort()
            };
        }

        public static void Write(NetDataWriter writer, MessageConfigurationMessage dto)
        {
            writer.Put(dto.Id);
            writer.Put(dto.Name);
            writer.Put(dto.DataTypeId);
        }
        #endregion
    }
}
