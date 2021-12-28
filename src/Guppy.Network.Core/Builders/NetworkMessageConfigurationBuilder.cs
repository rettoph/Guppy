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
        #region Static Fields
        public static readonly MessageBus.Queue DefaultMessageBusQueue = new MessageBus.Queue("default-network-message-queue", 0);
        #endregion

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
    public abstract class NetworkMessageConfigurationBuilder<TData, TNetworkMessageConfigurationBuilder> : NetworkMessageConfigurationBuilder, IFluentPrioritizable<NetworkMessageConfigurationBuilder<TData, TNetworkMessageConfigurationBuilder>>
        where TData : class, IData
        where TNetworkMessageConfigurationBuilder : NetworkMessageConfigurationBuilder<TData, TNetworkMessageConfigurationBuilder>
    {
        #region Protected Properties
        protected ServiceProviderBuilder services { get; private set; }
        protected NetworkProviderBuilder network { get; private set; }
        #endregion

        #region Public Properties
        public DeliveryMethod DeliveryMethod { get; set; }
        public Byte SequenceChannel { get; set; }
        public String ProcessorConfigurationName { get; set; }
        public Func<ServiceProvider, NetworkMessageConfiguration, Boolean> Filter { get; set; }
        public MessageBus.Queue MessageBusQueue { get; set; }
        #endregion

        #region Constructor
        internal NetworkMessageConfigurationBuilder(NetworkProviderBuilder network, ServiceProviderBuilder services) : base(typeof(TData))
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
        /// Define the service name to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder SetProcessorConfiguration(String processorServiceConfigurationName)
        {
            this.ProcessorConfigurationName = processorServiceConfigurationName;

            return this as TNetworkMessageConfigurationBuilder;
        }
        /// <summary>
        /// Define the service type to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder SetProcessorConfiguration<TMessageProcessor>()
            where TMessageProcessor : IMessageProcessor<TData>
        {
            return this.SetProcessorConfiguration(typeof(TMessageProcessor).FullName);
        }

        #endregion

        #region RegisterProcessorServiceConfiguration Methods
        /// <summary>
        /// Register a new service to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        private TNetworkMessageConfigurationBuilder RegisterProcessorConfiguration<TMessageProcessor>(
            ServiceConfigurationBuilder<TMessageProcessor> service,
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IMessageProcessor<TData>
        {
            builder(service);
            this.SetProcessorConfiguration(service.Name);

            return this as TNetworkMessageConfigurationBuilder;
        }
        /// <summary>
        /// Register a new service to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder RegisterProcessorConfiguration<TMessageProcessor>(
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IMessageProcessor<TData>
        {
            return this.RegisterProcessorConfiguration(
                this.services.RegisterService<TMessageProcessor>(), 
                builder);
        }
        /// <summary>
        /// Register a new service to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public TNetworkMessageConfigurationBuilder RegisterProcessorConfiguration<TMessageProcessor>(
            String name,
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IMessageProcessor<TData>
        {
            return this.RegisterProcessorConfiguration(
                this.services.RegisterService<TMessageProcessor>(name),
                builder);
        }
        #endregion

        #region SetNetworkAuthorization Methods
        public TNetworkMessageConfigurationBuilder SetFilter(Func<ServiceProvider, NetworkMessageConfiguration, Boolean> filter)
        {
            this.Filter = filter;

            return this as TNetworkMessageConfigurationBuilder;
        }

        public TNetworkMessageConfigurationBuilder SetPeerFilter<TPeer>()
            where TPeer : Peer
        {
            return this.SetFilter((p, _) => {
                var peer = p.GetService<Peer>();

                return peer is TPeer;
            });
        }
        #endregion

        #region SetMessageBusQueue Methods
        public TNetworkMessageConfigurationBuilder SetMessageBusQueue(MessageBus.Queue queue)
        {
            this.MessageBusQueue = queue;

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region RegisterMessage Methods
        public TNetworkMessageConfigurationBuilder RegisterDataType(Action<DataConfigurationBuilder<TData>> builder)
        {
            DataConfigurationBuilder<TData> dataType = this.network.RegisterDataType<TData>();
            builder(dataType);

            return this as TNetworkMessageConfigurationBuilder;
        }
        #endregion

        #region Build Methods
        public override NetworkMessageConfiguration Build(
            DynamicId id,
            DoubleDictionary<UInt16, Type, DataConfiguration> dataTypeConfigurations)
        {
            
            return new NetworkMessageConfiguration<TData>(
                id,
                dataTypeConfigurations[this.DataConfigurationType],
                this.DeliveryMethod,
                this.SequenceChannel,
                this.ProcessorConfigurationName,
                this.Filter ?? DefaultFilter,
                this.MessageBusQueue ?? DefaultMessageBusQueue);
        }

        private static Boolean DefaultFilter(ServiceProvider providert, NetworkMessageConfiguration configuration)
        {
            return true;
        }
        #endregion
    }

    public class NetworkMessageConfigurationBuilder<TData> : NetworkMessageConfigurationBuilder<TData, NetworkMessageConfigurationBuilder<TData>>
        where TData : class, IData
    {
        internal NetworkMessageConfigurationBuilder(NetworkProviderBuilder network, ServiceProviderBuilder services) : base(network, services)
        {
        }
    }
}
