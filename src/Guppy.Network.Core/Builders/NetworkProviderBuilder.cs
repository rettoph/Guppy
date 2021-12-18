using Guppy.EntityComponent.DependencyInjection;
using Minnow.General;
using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Builders
{
    public class NetworkProviderBuilder : IDisposable
    {
        #region Private Fields
        private List<DataTypeConfigurationBuilder> _dataTypes;
        private List<MessageConfigurationBuilder> _messages;
        #endregion

        #region Constructors
        public NetworkProviderBuilder()
        {
            _dataTypes = new List<DataTypeConfigurationBuilder>();
            _messages = new List<MessageConfigurationBuilder>();
        }
        #endregion

        #region RegisterDataType Methods
        public DataTypeConfigurationBuilder<IData> RegisterDataType(Type type)
        {
            typeof(IData).ValidateAssignableFrom(type);

            DataTypeConfigurationBuilder<IData> dataType = new DataTypeConfigurationBuilder<IData>(type, this);
            _dataTypes.Add(dataType);

            return dataType;
        }

        public DataTypeConfigurationBuilder<TData> RegisterDataType<TData>()
            where TData : class, IData
        {
            DataTypeConfigurationBuilder<TData> dataType = new DataTypeConfigurationBuilder<TData>(typeof(TData), this);
            _dataTypes.Add(dataType);

            return dataType;
        }
        #endregion

        #region RegisterMessage Methods
        public MessageConfigurationBuilder<TData> RegisterMessage<TData>(String name)
            where TData : class, IData
        {
            MessageConfigurationBuilder<TData> message = new MessageConfigurationBuilder<TData>(name);
            _messages.Add(message);

            return message;
        }

        public MessageConfigurationBuilder<IData> RegisterMessage(String name)
        {
            return this.RegisterMessage<IData>(name);
        }
        #endregion

        #region Build Methods
        private void BuildDataTypes(
            out DynamicIdSize dataTypeIdSize,
            out DoubleDictionary<UInt16, Type, DataTypeConfiguration> dataTypes)
        {
            // Calculate the maximum id
            UInt16 maxDataTypeId = (UInt16)(_dataTypes.Any() ? _dataTypes.Count - 1 : 0);

            // Determin the id size based on the maximum id
            dataTypeIdSize = DynamicId.GetSize(maxDataTypeId);

            // Create a 0 id that can be incremented
            DynamicId id = new DynamicId(0, dataTypeIdSize);

            // Create the DoubleDictionary, making sure to auto incrememnt the id as configurations build. 
            dataTypes =  new DoubleDictionary<UInt16, Type, DataTypeConfiguration>(
                keySelector1: dtc => dtc.Id.Value,
                keySelector2: dtc => dtc.Type,
                values: _dataTypes.PrioritizeBy(dtcb => dtcb.Type).Select(dtcb => dtcb.Build(id++)).ToList());
        }

        private void BuildMessages(
            DoubleDictionary<UInt16, Type, DataTypeConfiguration> dataTypes,
            out DynamicIdSize messageIdSize,
            out DoubleDictionary<UInt16, String, MessageConfiguration> messages)
        {
            // Calculate the maximum id
            UInt16 maxMessageId = (UInt16)(_messages.Any() ? _messages.Count - 1 : 0);

            // Determin the id size based on the maximum id
            messageIdSize = DynamicId.GetSize(maxMessageId);

            // Create a 0 id that can be incremented
            DynamicId id = new DynamicId(0, messageIdSize);

            // Create the DoubleDictionary, making sure to auto incrememnt the id as configurations build. 
            messages = new DoubleDictionary<UInt16, String, MessageConfiguration>(
                keySelector1: mc => mc.Id.Value,
                keySelector2: mc => mc.Name,
                values: _messages.PrioritizeBy(mcb => mcb.Name).Select(mcb => mcb.Build(id++, dataTypes)).ToList());
        }

        public NetworkProvider Build()
        {
            this.BuildDataTypes(
                out DynamicIdSize dataTypesIdSize,
                out DoubleDictionary<UInt16, Type, DataTypeConfiguration> dataTypes);

            this.BuildMessages(
                dataTypes,
                out DynamicIdSize messageIdSize,
                out DoubleDictionary<UInt16, String, MessageConfiguration> messages);

            return new NetworkProvider(
                dataTypesIdSize,
                dataTypes,
                messageIdSize,
                messages);
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            _dataTypes.Clear();
            _messages.Clear();
        }
        #endregion
    }
}
