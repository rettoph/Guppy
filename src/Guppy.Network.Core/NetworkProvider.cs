using Minnow.General;
using Guppy.Network.Configurations;
using Guppy.Network.Messages;
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
        private DoubleDictionary<UInt16, Type, DataConfiguration> _dataTypes;

        private DynamicIdSize _messagesIdSize;
        private Action<NetDataWriter, Byte[]> _messageIdWriter;
        private Func<NetDataReader, UInt16> _messageIdReader;
        private DoubleDictionary<UInt16, Type, NetworkMessageConfiguration> _messages;

        public IEnumerable<DataConfiguration> DataTypeConfigurations => _dataTypes.Values;
        public IEnumerable<NetworkMessageConfiguration> MessageConfigurations => _messages.Values;

        internal NetworkProvider(
            DynamicIdSize dataTypesIdSize,
            DoubleDictionary<UInt16, Type, DataConfiguration> dataTypes,
            DynamicIdSize messagesIdSize,
            DoubleDictionary<UInt16, Type, NetworkMessageConfiguration> messages)
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
        public DataConfiguration GetDataTypeConfiguration<TDataType>()
            where TDataType : IData
        {
            return _dataTypes[typeof(TDataType)];
        }
        public DataConfiguration GetDataTypeConfiguration(Type type)
        {
            return _dataTypes[type];
        }
        public DataConfiguration GetDataTypeConfiguration(UInt16 id)
        {
            return _dataTypes[id];
        }
        #endregion

        #region GetMessageConfiguration Methods
        public NetworkMessageConfiguration GetMessageConfiguration(Type type)
        {
            return _messages[type];
        }
        public NetworkMessageConfiguration GetMessageConfiguration(UInt16 id)
        {
            return _messages[id];
        }
        #endregion

        #region SendMessage Methods
        public void SendMessage<TData>(NetDataWriter writer, TData data, NetPeer recipient)
            where TData : class, IData
        {
            NetworkMessageConfiguration configuration = _messages[typeof(TData)];

            // Write message id...
            _messageIdWriter(writer, configuration.Id.Bytes);
            // Write message data...
            configuration.DataConfiguration.Writer(writer, data);

            // Send message
            recipient.Send(writer, configuration.SequenceChannel, configuration.DeliveryMethod);
        }
        #endregion

        #region ReadMessage
        public NetworkMessage ReadMessage(NetDataReader reader)
        {
            UInt16 messageId = _messageIdReader(reader);
            NetworkMessageConfiguration configuration = _messages[messageId];

            return new NetworkMessage()
            {
                Configuration = configuration,
                Data = configuration.DataConfiguration.Reader(reader)
            };
        }
        #endregion

        #region Helper Methods
        internal NetworkProviderMessage GetMessage()
        {
            return new NetworkProviderMessage()
            {
                DataTypesIdSize = _dataTypesIdSize,
                DataTypeConfigurations = _dataTypes.Values.Select(dt => dt.GetMessage()),
                MessagesIdSize = _messagesIdSize,
                MessageConfigurations = _messages.Values.Select(m => m.GetMessage())
            };
        }

        /// <summary>
        /// Compare a recieved dto and ensure the data contained matches the current
        /// provider configuration exactly.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal Boolean CheckDto(NetworkProviderMessage dto)
        {
            #region Check Data Types
            if (_dataTypesIdSize != dto.DataTypesIdSize)
            {
                return false;
            }

            foreach(DataTypeConfigurationMessage dataTypeDto in dto.DataTypeConfigurations)
            {
                if (!_dataTypes.TryGetValue(dataTypeDto.Id, out DataConfiguration dataType)
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

            foreach(MessageConfigurationMessage messageDto in dto.MessageConfigurations)
            {
                if (!_messages.TryGetValue(messageDto.Id, out NetworkMessageConfiguration message)
                    || message.DataConfiguration.Id != messageDto.DataTypeId)
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
