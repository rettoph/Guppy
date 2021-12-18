using Guppy.EntityComponent.DependencyInjection;
using Minnow.General;
using Minnow.General.Interfaces;
using Guppy.Network.Configurations;
using Guppy.Network.MessageProcessors;
using Guppy.Network.Interfaces;
using Guppy.Network.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using Guppy.Network.Enums;

namespace Guppy.Network.Builders
{
    public abstract class MessageConfigurationBuilder : IPrioritizable
    {
        #region Public Properties
        public String Name { get; }

        public Int32 Priority { get; set; }
        #endregion

        #region Constructor
        internal MessageConfigurationBuilder(String name)
        {
            this.Name = name;
        }
        #endregion

        #region Build Methods
        public abstract MessageConfiguration Build(
            DynamicId id,
            DoubleDictionary<UInt16, Type, DataTypeConfiguration> dataTypeConfigurations);
        #endregion
    }
    public class MessageConfigurationBuilder<TData> : MessageConfigurationBuilder, IFluentPrioritizable<MessageConfigurationBuilder<TData>>
        where TData : class, IData
    {
        #region Private Fields
        private Type _dataType;
        #endregion

        #region Public Properties
        public Type DataType
        {
            get => _dataType;
            set => this.SetDataType(value);
        }

        public DeliveryMethod DeliveryMethod { get; set; }
        public Byte SequenceChannel { get; set; }

        public Func<ServiceProvider, MessageProcessor<TData>> ProcessorFactory { get; set; }

        public Func<ServiceProvider, MessageConfiguration, Boolean> Filter { get; set; }
        #endregion

        #region Constructor
        internal MessageConfigurationBuilder(String name) : base(name)
        {
        }
        #endregion

        #region SetType Methods
        public MessageConfigurationBuilder<TData> SetDataType(Type dataType)
        {
            if (typeof(TData).ValidateAssignableFrom(dataType))
            {
                _dataType = dataType;
            }

            return this;
        }
        #endregion

        #region SetDeliveryMethod Methods
        public MessageConfigurationBuilder<TData> SetDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            this.DeliveryMethod = deliveryMethod;

            return this;
        }
        #endregion

        #region SetSequenceChannel Methods
        public MessageConfigurationBuilder<TData> SetSequenceChannel(Byte sequenceChannel)
        {
            this.SequenceChannel = sequenceChannel;

            return this;
        }
        #endregion

        #region SetProcessorFactory Methods
        public MessageConfigurationBuilder<TData> SetProcessorFactory<TMessageProcessor>(Func<ServiceProvider, TMessageProcessor> processorFactory)
            where TMessageProcessor : MessageProcessor<TData>
        {
            this.ProcessorFactory = processorFactory;

            return this;
        }
        #endregion

        #region SetNetworkAuthorization Methods
        public MessageConfigurationBuilder<TData> SetFilter(Func<ServiceProvider, MessageConfiguration, Boolean> filter)
        {
            this.Filter = filter;

            return this;
        }

        public MessageConfigurationBuilder<TData> SetPeerFilter<TPeer>()
            where TPeer : Peer
        {
            return this.SetFilter((p, _) => p.GetService<Peer>() is TPeer);
        }
        #endregion

        #region Build Methods
        public override MessageConfiguration Build(
            DynamicId id,
            DoubleDictionary<UInt16, Type, DataTypeConfiguration> dataTypeConfigurations)
        {
            Func<ServiceProvider, MessageProcessor> processorFactory = this.ProcessorFactory;

            return new MessageConfiguration(
                id,
                this.Name,
                dataTypeConfigurations[this.DataType ?? typeof(TData)],
                this.DeliveryMethod,
                this.SequenceChannel,
                processorFactory ?? EmptyIncomingMessageResponseMessageProcessor.Factory,
                this.Filter ?? DefaultFilter);
        }

        private static Boolean DefaultFilter(ServiceProvider providert, MessageConfiguration configuration)
        {
            return true;
        }
        #endregion
    }
}
