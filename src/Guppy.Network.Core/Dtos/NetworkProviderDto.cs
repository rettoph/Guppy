using Guppy.Network.Enums;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Dtos
{
    internal sealed class NetworkProviderDto
    {
        public DynamicIdSize DataTypesIdSize { get; internal set; }
        public IEnumerable<DataTypeConfigurationDto> DataTypeConfigurations { get; internal set; }


        public DynamicIdSize MessagesIdSize { get; internal set; }
        public IEnumerable<MessageConfigurationDto> MessageConfigurations { get; internal set; }

        #region Read/Write Methods
        public static NetworkProviderDto Read(NetDataReader reader)
        {
            DynamicIdSize dataTypesIdSize = reader.GetEnum<DynamicIdSize>();

            List<DataTypeConfigurationDto> dataTypeConfigurations = new List<DataTypeConfigurationDto>();
            while (reader.GetBool())
            {
                dataTypeConfigurations.Add(DataTypeConfigurationDto.Read(reader));
            }

            DynamicIdSize messagesIdSize = reader.GetEnum<DynamicIdSize>();

            List<MessageConfigurationDto> messageConfigurations = new List<MessageConfigurationDto>();
            while (reader.GetBool())
            {
                messageConfigurations.Add(MessageConfigurationDto.Read(reader));
            }

            return new NetworkProviderDto()
            {
                DataTypesIdSize = dataTypesIdSize,
                DataTypeConfigurations = dataTypeConfigurations,
                MessagesIdSize = messagesIdSize,
                MessageConfigurations = messageConfigurations
            };
        }

        public static void Write(NetDataWriter writer, NetworkProviderDto dto)
        {
            writer.Put(dto.DataTypesIdSize);

            foreach(DataTypeConfigurationDto dataType in dto.DataTypeConfigurations)
            {
                writer.Put(true);
                DataTypeConfigurationDto.Write(writer, dataType);
            }
            writer.Put(false);

            writer.Put(dto.MessagesIdSize);

            foreach (MessageConfigurationDto message in dto.MessageConfigurations)
            {
                writer.Put(true);
                MessageConfigurationDto.Write(writer, message);
            }
            writer.Put(false);
        }
        #endregion
    }
}
