using Minnow.General;
using Guppy.Network.Configurations;
using Guppy.Network.Dtos;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network
{
    public sealed class NetworkProvider
    {
        private DynamicIdSize _dataTypesIdSize;
        private Action<NetDataWriter, Byte[]> _dataTypeIdWriter;
        private Func<NetDataReader, UInt16> _dataTypeIdReader;
        private DoubleDictionary<UInt16, Type, DataTypeConfiguration> _dataTypes;

        private DynamicIdSize _messagesIdSize;
        private Action<NetDataWriter, Byte[]> _messageIdWriter;
        private Func<NetDataReader, UInt16> _messageIdReader;
        private DoubleDictionary<UInt16, String, MessageConfiguration> _messages;

        public IEnumerable<DataTypeConfiguration> DataTypeConfigurations => _dataTypes.Values;
        public IEnumerable<MessageConfiguration> MessageConfigurations => _messages.Values;

        internal NetworkProvider(
            DynamicIdSize dataTypesIdSize,
            DoubleDictionary<UInt16, Type, DataTypeConfiguration> dataTypes,
            DynamicIdSize messagesIdSize,
            DoubleDictionary<UInt16, String, MessageConfiguration> messages)
        {
            _dataTypesIdSize = dataTypesIdSize;
            _dataTypeIdWriter = DynamicId.GetNetWriter(_dataTypesIdSize);
            _dataTypeIdReader = DynamicId.GetNetReader(_dataTypesIdSize);
            _dataTypes = dataTypes;

            _messagesIdSize = messagesIdSize;
            _messageIdWriter = DynamicId.GetNetWriter(_messagesIdSize);
            _messageIdReader = DynamicId.GetNetReader(_messagesIdSize);
            _messages = messages;
        }

        #region GetDataTypeConfiguration Methods
        public DataTypeConfiguration GetDataTypeConfiguration<TDataType>()
            where TDataType : IData
        {
            return _dataTypes[typeof(TDataType)];
        }
        public DataTypeConfiguration GetDataTypeConfiguration(Type type)
        {
            return _dataTypes[type];
        }
        public DataTypeConfiguration GetDataTypeConfiguration(UInt16 id)
        {
            return _dataTypes[id];
        }
        #endregion

        #region GetMessageConfiguration Methods
        public MessageConfiguration GetMessageConfiguration(String name)
        {
            return _messages[name];
        }
        public MessageConfiguration GetMessageConfiguration(UInt16 id)
        {
            return _messages[id];
        }
        #endregion

        #region SendMessage Methods
        public void SendMessage(NetDataWriter writer, String messageName, IData data, NetPeer recipient)
        {
            MessageConfiguration configuration = _messages[messageName];

            // Write message id...
            _messageIdWriter(writer, configuration.Id.Bytes);
            // Write message data...
            configuration.DataType.Writer(writer, data);

            // Send message
            recipient.Send(writer, configuration.SequenceChannel, configuration.DeliveryMethod);
        }
        #endregion

        #region ReadMessage
        public Message ReadMessage(NetDataReader reader)
        {
            UInt16 messageId = _messageIdReader(reader);
            MessageConfiguration configuration = _messages[messageId];

            return new Message()
            {
                Configuration = configuration,
                Data = configuration.DataType.Reader(reader)
            };
        }
        #endregion

        #region Helper Methods
        internal NetworkProviderDto ToDto()
        {
            return new NetworkProviderDto()
            {
                DataTypesIdSize = _dataTypesIdSize,
                DataTypeConfigurations = _dataTypes.Values.Select(dt => dt.ToDto()),
                MessagesIdSize = _messagesIdSize,
                MessageConfigurations = _messages.Values.Select(m => m.ToDto())
            };
        }

        /// <summary>
        /// Compare a recieved dto and ensure the data contained matches the current
        /// provider configuration exactly.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal Boolean CheckDto(NetworkProviderDto dto)
        {
            #region Check Data Types
            if (_dataTypesIdSize != dto.DataTypesIdSize)
            {
                return false;
            }

            foreach(DataTypeConfigurationDto dataTypeDto in dto.DataTypeConfigurations)
            {
                if (!_dataTypes.TryGetValue(dataTypeDto.Id, out DataTypeConfiguration dataType)
                    || dataType.Type.AssemblyQualifiedName != dataTypeDto.TypeAssemblyQualifiedName)
                {
                    return false;
                }
            }
            #endregion

            #region Check Messages
            if (_messagesIdSize != dto.MessagesIdSize)
            {
                return false;
            }

            foreach(MessageConfigurationDto messageDto in dto.MessageConfigurations)
            {
                if (!_messages.TryGetValue(messageDto.Id, out MessageConfiguration message)
                    || message.Name != messageDto.Name
                    || message.DataType.Id != messageDto.DataTypeId)
                {
                    return false;
                }
            }
            #endregion

            // If we've made it this far then the configurations match!
            return true;
        }
        #endregion
    }
}
