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
using Guppy.Network.Utilities;

namespace Guppy.Network
{
    public sealed class NetworkProvider
    {
        #region Private Fields
        private DynamicIdSize _dataTypesIdSize;
        private Action<NetDataWriter, Byte[]> _dataConfigurationIdWriter;
        private Func<NetDataReader, UInt16> _dataConfigurationIdReader;
        private DoubleDictionary<UInt16, Type, DataConfiguration> _dataConfigurations;

        private DynamicIdSize _messagesIdSize;
        private Action<NetDataWriter, Byte[]> _messageIdWriter;
        private Func<NetDataReader, UInt16> _messageIdReader;
        private DoubleDictionary<UInt16, Type, NetworkMessageConfiguration> _messages;

        private NetDataWriterFactory _netDataWriterFactory;
        #endregion

        #region Public Properties
        /// <summary>
        /// QoS channel count per message type (value must be between 1 and 64 channels)
        /// </summary>
        public readonly Byte SequenceChannelCount;
        public IEnumerable<DataConfiguration> DataTypeConfigurations => _dataConfigurations.Values;
        public IEnumerable<NetworkMessageConfiguration> MessageConfigurations => _messages.Values;
        #endregion

        #region Constructor
        internal NetworkProvider(
            Byte sequenceChannelCount,
            DynamicIdSize dataTypesIdSize,
            DoubleDictionary<UInt16, Type, DataConfiguration> dataTypes,
            DynamicIdSize messagesIdSize,
            DoubleDictionary<UInt16, Type, NetworkMessageConfiguration> messages)
        {
            _dataTypesIdSize = dataTypesIdSize;
            _dataConfigurationIdWriter = DynamicId.GetNetWriter(_dataTypesIdSize);
            _dataConfigurationIdReader = DynamicId.GetNetReader(_dataTypesIdSize);
            _dataConfigurations = dataTypes;

            _messagesIdSize = messagesIdSize;
            _messageIdWriter = DynamicId.GetNetWriter(_messagesIdSize);
            _messageIdReader = DynamicId.GetNetReader(_messagesIdSize);
            _messages = messages;

            _netDataWriterFactory = new NetDataWriterFactory();

            this.SequenceChannelCount = sequenceChannelCount;
        }
        #endregion

        #region GetDataConfiguration Methods
        public DataConfiguration GetDataConfiguration<TDataType>()
            where TDataType : IData
        {
            return _dataConfigurations[typeof(TDataType)];
        }
        public DataConfiguration GetDataConfiguration(Type type)
        {
            return _dataConfigurations[type];
        }
        public DataConfiguration GetDataConfiguration(IData data)
        {
            return _dataConfigurations[data.GetType()];
        }
        public DataConfiguration GetDataTypeConfiguration(UInt16 id)
        {
            return _dataConfigurations[id];
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
        public void SendMessage<TData>(Room room, TData data, NetPeer recipient)
            where TData : class, IData
        {
            this.WriteMessage(room, data, out NetDataWriter writer, out NetworkMessageConfiguration configuration);

            recipient.Send(writer, configuration.SequenceChannel, configuration.DeliveryMethod);

            this.RecycleMessage(writer);
            data.Clean();
        }
        public void SendMessage<TData>(Room room, TData data, IEnumerable<NetPeer> recipients)
            where TData : class, IData
        {
            this.WriteMessage(room, data, out NetDataWriter writer, out NetworkMessageConfiguration configuration);

            foreach (NetPeer recipient in recipients)
            {
                recipient.Send(writer, configuration.SequenceChannel, configuration.DeliveryMethod);
            }

            this.RecycleMessage(writer);
            data.Clean();
        }
        #endregion

        #region WriteMessage Methods
        public void WriteMessage<TData>(Room room, TData data, out NetDataWriter writer, out NetworkMessageConfiguration configuration)
            where TData : class, IData
        {
            writer = _netDataWriterFactory.GetInstance();
            configuration = _messages[typeof(TData)];

            // Write message id...
            _messageIdWriter(writer, configuration.Id.Bytes);
            // Write RoomId
            writer.Put(room.Id);
            // Write message data...
            configuration.DataConfiguration.Writer(writer, this, data);
        }
        #endregion

        #region ReadMessage Methods
        public NetworkMessage ReadMessage(NetDataReader reader)
        {
            UInt16 messageId = _messageIdReader(reader);
            NetworkMessageConfiguration configuration = _messages[messageId];

            return new NetworkMessage()
            {
                RoomId = reader.GetByte(),
                Configuration = configuration,
                Data = configuration.DataConfiguration.Reader(reader, this)
            };
        }
        #endregion

        #region WriteData Methods
        public void WriteData(NetDataWriter writer, IData data)
        {
            DataConfiguration configuration = _dataConfigurations[data.GetType()];

            // Write data id...
            _dataConfigurationIdWriter(writer, configuration.Id.Bytes);
            // Write data...
            configuration.Writer(writer, this, data);
        }
        #endregion

        #region ReadData Methods
        public TData ReadData<TData>(NetDataReader reader)
            where TData : class, IData
        {
            UInt16 dataConfigurationId = _dataConfigurationIdReader(reader);
            return _dataConfigurations[dataConfigurationId].Reader(reader, this) as TData;
        }
        #endregion

        #region RecycleMethods Methods
        public void RecycleMessage(NetDataWriter writer)
        {
            _netDataWriterFactory.TryReturnToPool(writer);
        }
        #endregion

        #region Helper Methods
        internal NetworkProviderMessage ToMessage()
        {
            return new NetworkProviderMessage()
            {
                DataTypesIdSize = _dataTypesIdSize,
                DataTypeConfigurations = _dataConfigurations.Values.Select(dt => dt.ToMessage()),
                MessagesIdSize = _messagesIdSize,
                MessageConfigurations = _messages.Values.Select(m => m.ToMessage())
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
                if (!_dataConfigurations.TryGetValue(dataTypeDto.Id, out DataConfiguration dataType)
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
