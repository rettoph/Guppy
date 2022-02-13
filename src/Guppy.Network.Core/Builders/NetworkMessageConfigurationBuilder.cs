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
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;

namespace Guppy.Network.Builders
{
    public abstract class NetworkMessageConfigurationBuilder : IPrioritizable
    {
        #region Public Fields
        public readonly Type DataConfigurationType;
        #endregion

        #region Public Properties
        public Int32 Priority { get; set; }
        #endregion

        #region Constructors
        protected NetworkMessageConfigurationBuilder(Type dataType)
        {
            this.DataConfigurationType = dataType;
        }
        #endregion

        #region Build Methods
        public abstract NetworkMessageConfiguration Build(
            DynamicId id,
            DoubleDictionary<UInt16, Type, DataConfiguration> dataTypeConfigurations);
        #endregion
    }
    public abstract class NetworkMessageConfigurationBuilder<TMessage, TNetworkMessageConfigurationBuilder> : NetworkMessageConfigurationBuilder, IFluentPrioritizable<NetworkMessageConfigurationBuilder<TMessage, TNetworkMessageConfigurationBuilder>>
        where TMessage : class, IMessage
        where TNetworkMessageConfigurationBuilder : NetworkMessageConfigurationBuilder<TMessage, TNetworkMessageConfigurationBuilder>
    {
        #region Protected Properties
        protected ServiceProviderBuilder services { get; private set; }
        protected NetworkProviderBuilder network { get; private set; }
        #endregion

        #region Public Properties
        public DeliveryMethod DeliveryMethod { get; set; }
        public Byte SequenceChannel { get; set; }
        public Type ProcessorConfigurationType { get; set; }
        public Int32? IncomingPriority { get; set; }
        public Int32? OutgoingPriority { get; set; }
        public NetworkAuthorization? SenderAuthorization { get; set; }
        #endregion

        #region Constructor
        internal NetworkMessageConfigurationBuilder(NetworkProviderBuilder network, ServiceProviderBuilder services) : base(typeof(TMessage))
        {
            this.network = network;
            this.services = services;
        }
        #endregion

        #region SetDeliveryMethod Methods
        public TNetworkMessageConfigurationBuilder SetDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            this.DeliveryMethod = deliveryMethod;

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region SetSequenceChannel Methods
        public TNetworkMessageConfigurationBuilder SetSequenceChannel(Byte sequenceChannel)
        {
            this.SequenceChannel = sequenceChannel;

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region SetProcessorConfiguration Methods
        /// <summary>
        /// Define the service name to act as the message's <see cref="IDataProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder SetProcessorConfiguration(Type processorServiceConfigurationType)
        {
            this.ProcessorConfigurationType = processorServiceConfigurationType;

            return this as TNetworkMessageConfigurationBuilder;
        }
        /// <summary>
        /// Define the service type to act as the message's <see cref="IDataProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder SetProcessorConfiguration<TMessageProcessor>()
            where TMessageProcessor : IDataProcessor<TMessage>
        {
            return this.SetProcessorConfiguration(typeof(TMessageProcessor));
        }

        #endregion

        #region RegisterProcessorServiceConfiguration Methods
        /// <summary>
        /// Register a new service to act as the message's <see cref="IDataProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        private TNetworkMessageConfigurationBuilder RegisterProcessorConfiguration<TMessageProcessor>(
            ServiceConfigurationBuilder<TMessageProcessor> service,
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IDataProcessor<TMessage>
        {
            builder(service);
            this.SetProcessorConfiguration(service.Type);

            return this as TNetworkMessageConfigurationBuilder;
        }
        /// <summary>
        /// Register a new service to act as the message's <see cref="IDataProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder RegisterProcessorConfiguration<TMessageProcessor>(
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IDataProcessor<TMessage>
        {
            return this.RegisterProcessorConfiguration(
                this.services.RegisterService<TMessageProcessor>(), 
                builder);
        }
        /// <summary>
        /// Register a new service to act as the message's <see cref="IDataProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder RegisterProcessorConfiguration<TMessageProcessor>(
            Type type,
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IDataProcessor<TMessage>
        {
            return this.RegisterProcessorConfiguration(
                this.services.RegisterService<TMessageProcessor>(type),
                builder);
        }
        #endregion

        #region SetNetworkAuthorization Methods
        public TNetworkMessageConfigurationBuilder SetSenderAuthorization(NetworkAuthorization senderAuthorization)
        {
            this.SenderAuthorization = senderAuthorization;

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region SetIncomingMessageBusQueue Methods
        public TNetworkMessageConfigurationBuilder SetIncomingPriority(Int32 priority)
        {
            this.IncomingPriority = priority;

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region SetOutogingMessageBusQueue Methods
        public TNetworkMessageConfigurationBuilder SetOutgoingPriority(Int32 priority)
        {
            this.OutgoingPriority = priority;

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region RegisterMessage Methods
        public TNetworkMessageConfigurationBuilder RegisterDataType(Action<DataConfigurationBuilder<TMessage>> builder)
        {
            DataConfigurationBuilder<TMessage> dataType = this.network.RegisterDataType<TMessage>();
            builder(dataType);

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region Build Methods
        public override NetworkMessageConfiguration Build(
            DynamicId id,
            DoubleDictionary<UInt16, Type, DataConfiguration> dataTypeConfigurations)
        {
            return new NetworkMessageConfiguration<TMessage>(
                id,
                dataTypeConfigurations[this.DataConfigurationType],
                this.DeliveryMethod,
                this.SequenceChannel,
                this.ProcessorConfigurationType,
                this.SenderAuthorization ?? NetworkAuthorization.Slave | NetworkAuthorization.Master,
                this.IncomingPriority ?? Constants.Queues.DefaultIncomingMessagePriority,
                this.OutgoingPriority ?? Constants.Queues.DefaultOutgoingMessagePriority);
        }
        #endregion
    }

    public class NetworkMessageConfigurationBuilder<TMessage> : NetworkMessageConfigurationBuilder<TMessage, NetworkMessageConfigurationBuilder<TMessage>>
        where TMessage : class, IMessage
    {
        internal NetworkMessageConfigurationBuilder(NetworkProviderBuilder network, ServiceProviderBuilder services) : base(network, services)
        {
        }
    }
}
