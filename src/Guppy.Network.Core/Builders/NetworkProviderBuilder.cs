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
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Network.Messages;

namespace Guppy.Network.Builders
{
    public class NetworkProviderBuilder : IDisposable
    {
        #region Private Fields
        private List<DataConfigurationBuilder> _dataTypes;
        private List<NetworkMessageConfigurationBuilder> _messages;
        private ServiceProviderBuilder _services;
        #endregion

        #region Public Properties
        public Byte SequenceChannelCount { get; set; }
        #endregion

        #region Constructors
        public NetworkProviderBuilder(ServiceProviderBuilder service)
        {
            this.SequenceChannelCount = 1;

            _dataTypes = new List<DataConfigurationBuilder>();
            _messages = new List<NetworkMessageConfigurationBuilder>();
            _services = service;
        }
        #endregion

        #region RegisterDataType Methods
        public DataConfigurationBuilder<TData> RegisterDataType<TData>()
            where TData : class, IData
        {
            DataConfigurationBuilder<TData> dataType = new DataConfigurationBuilder<TData>(this);
            _dataTypes.Add(dataType);

            return dataType;
        }
        #endregion

        #region RegisterNetworkMessage Methods
        public NetworkMessageConfigurationBuilder<TData> RegisterNetworkMessage<TData>()
            where TData : class, IData
        {
            NetworkMessageConfigurationBuilder<TData> message = new NetworkMessageConfigurationBuilder<TData>(_services);
            _messages.Add(message);

            return message;
        }
        #endregion

        #region RegisterNetworkEntityMessage Methods
        /// <summary>
        /// Add a new message configuration designed specifically for communicating to Network Entities.
        /// This will automatically define a DataType & processor. Dont change these unless you know 
        /// what you're doing.
        /// </summary>
        /// <typeparam name="TNetworkEntityMessage"></typeparam>
        /// <returns></returns>
        public NetworkEntityMessageConfigurationBuilder<TNetworkEntityMessage> RegisterNetworkEntityMessage<TNetworkEntityMessage>()
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            NetworkEntityMessageConfigurationBuilder<TNetworkEntityMessage> message = new NetworkEntityMessageConfigurationBuilder<TNetworkEntityMessage>(this, _services);
            _messages.Add(message);

            return message;
        }
        #endregion

        #region Build Methods
        private void BuildDataTypes(
            out DynamicIdSize dataTypeIdSize,
            out DoubleDictionary<UInt16, Type, DataConfiguration> dataTypes)
        {
            // Calculate the maximum id
            UInt16 maxDataTypeId = (UInt16)(_dataTypes.Any() ? _dataTypes.Count - 1 : 0);

            // Determin the id size based on the maximum id
            dataTypeIdSize = DynamicId.GetSize(maxDataTypeId);

            // Create a 0 id that can be incremented
            DynamicId id = new DynamicId(0, dataTypeIdSize);

            // Create the DoubleDictionary, making sure to auto incrememnt the id as configurations build. 
            dataTypes =  new DoubleDictionary<UInt16, Type, DataConfiguration>(
                keySelector1: dtc => dtc.Id.Value,
                keySelector2: dtc => dtc.Type,
                values: _dataTypes.PrioritizeBy(dtcb => dtcb.Type).Select(dtcb => dtcb.Build(id++)).ToList());
        }

        private void BuildMessages(
            DoubleDictionary<UInt16, Type, DataConfiguration> dataTypes,
            out DynamicIdSize messageIdSize,
            out DoubleDictionary<UInt16, Type, NetworkMessageConfiguration> messages)
        {
            // Calculate the maximum id
            UInt16 maxMessageId = (UInt16)(_messages.Any() ? _messages.Count - 1 : 0);

            // Determin the id size based on the maximum id
            messageIdSize = DynamicId.GetSize(maxMessageId);

            // Create a 0 id that can be incremented
            DynamicId id = new DynamicId(0, messageIdSize);

            // Create the DoubleDictionary, making sure to auto incrememnt the id as configurations build. 
            messages = new DoubleDictionary<UInt16, Type, NetworkMessageConfiguration>(
                keySelector1: mc => mc.Id.Value,
                keySelector2: mc => mc.DataConfiguration.Type,
                values: _messages.PrioritizeBy(mcb => mcb.DataConfigurationType).Select(mcb => mcb.Build(id++, dataTypes)).ToList());
        }

        public NetworkProvider Build()
        {
            this.BuildDataTypes(
                out DynamicIdSize dataTypesIdSize,
                out DoubleDictionary<UInt16, Type, DataConfiguration> dataTypes);

            this.BuildMessages(
                dataTypes,
                out DynamicIdSize messageIdSize,
                out DoubleDictionary<UInt16, Type, NetworkMessageConfiguration> messages);

            return new NetworkProvider(
                this.SequenceChannelCount,
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
