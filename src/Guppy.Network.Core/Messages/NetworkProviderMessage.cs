using Guppy.Network.Enums;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Messages
{
    internal sealed class NetworkProviderMessage
    {
        public DynamicIdSize DataTypesIdSize { get; internal set; }
        public IEnumerable<DataTypeConfigurationMessage> DataTypeConfigurations { get; internal set; }


        public DynamicIdSize MessagesIdSize { get; internal set; }
        public IEnumerable<MessageConfigurationMessage> MessageConfigurations { get; internal set; }

        #region Read/Write Methods
        public static NetworkProviderMessage Read(NetDataReader reader)
        {
            DynamicIdSize dataTypesIdSize = reader.GetEnum<DynamicIdSize>();

            List<DataTypeConfigurationMessage> dataTypeConfigurations = new List<DataTypeConfigurationMessage>();
            while (reader.GetBool())
            {
                dataTypeConfigurations.Add(DataTypeConfigurationMessage.Read(reader));
            }

            DynamicIdSize messagesIdSize = reader.GetEnum<DynamicIdSize>();

            List<MessageConfigurationMessage> messageConfigurations = new List<MessageConfigurationMessage>();
            while (reader.GetBool())
            {
                messageConfigurations.Add(MessageConfigurationMessage.Read(reader));
            }

            return new NetworkProviderMessage()
            {
                DataTypesIdSize = dataTypesIdSize,
                DataTypeConfigurations = dataTypeConfigurations,
                MessagesIdSize = messagesIdSize,
                MessageConfigurations = messageConfigurations
            };
        }

        public static void Write(NetDataWriter writer, NetworkProviderMessage dto)
        {
            writer.Put(dto.DataTypesIdSize);

            foreach(DataTypeConfigurationMessage dataType in dto.DataTypeConfigurations)
            {
                writer.Put(true);
                DataTypeConfigurationMessage.Write(writer, dataType);
            }
            writer.Put(false);

            writer.Put(dto.MessagesIdSize);

            foreach (MessageConfigurationMessage message in dto.MessageConfigurations)
            {
                writer.Put(true);
                MessageConfigurationMessage.Write(writer, message);
            }
            writer.Put(false);
        }
        #endregion
    }
}
