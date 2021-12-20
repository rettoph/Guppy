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
    public class NetworkMessageConfigurationBuilder<TData> : NetworkMessageConfigurationBuilder, IFluentPrioritizable<NetworkMessageConfigurationBuilder<TData>>
        where TData : class, IData
    {
        #region Private Fields
        private ServiceProviderBuilder _services;
        #endregion

        #region Public Properties
        public DeliveryMethod DeliveryMethod { get; set; }
        public Byte SequenceChannel { get; set; }
        public String ProcessorConfigurationName { get; set; }
        public Func<ServiceProvider, NetworkMessageConfiguration, Boolean> Filter { get; set; }
        #endregion

        #region Constructor
        internal NetworkMessageConfigurationBuilder(ServiceProviderBuilder services) : base(typeof(TData))
        {
            _services = services;
        }
        #endregion

        #region SetDeliveryMethod Methods
        public NetworkMessageConfigurationBuilder<TData> SetDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            this.DeliveryMethod = deliveryMethod;

            return this;
        }
        #endregion

        #region SetSequenceChannel Methods
        public NetworkMessageConfigurationBuilder<TData> SetSequenceChannel(Byte sequenceChannel)
        {
            this.SequenceChannel = sequenceChannel;

            return this;
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
        public NetworkMessageConfigurationBuilder<TData> SetProcessorConfigurationName(String processorServiceConfigurationName)
        {
            this.ProcessorConfigurationName = processorServiceConfigurationName;

            return this;
        }
        /// <summary>
        /// Define the service type to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public NetworkMessageConfigurationBuilder<TData> SetProcessorConfigurationType<TMessageProcessor>()
            where TMessageProcessor : IMessageProcessor<TData>
        {
            return this.SetProcessorConfigurationName(typeof(TMessageProcessor).FullName);
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
        private NetworkMessageConfigurationBuilder<TData> RegisterProcessorConfiguration<TMessageProcessor>(
            ServiceConfigurationBuilder<TMessageProcessor> service,
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IMessageProcessor<TData>
        {
            builder(service);
            this.SetProcessorConfigurationName(service.Name);

            return this;
        }
        /// <summary>
        /// Register a new service to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public NetworkMessageConfigurationBuilder<TData> RegisterProcessorConfiguration<TMessageProcessor>(
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IMessageProcessor<TData>
        {
            return this.RegisterProcessorConfiguration(
                _services.RegisterService<TMessageProcessor>(), 
                builder);
        }
        /// <summary>
        /// Register a new service to act as the message's <see cref="IMessageProcessor{TMessage}"/>
        /// </summary>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="service"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public NetworkMessageConfigurationBuilder<TData> RegisterProcessorConfiguration<TMessageProcessor>(
            String name,
            Action<ServiceConfigurationBuilder<TMessageProcessor>> builder)
            where TMessageProcessor : class, IMessageProcessor<TData>
        {
            return this.RegisterProcessorConfiguration(
                _services.RegisterService<TMessageProcessor>(name),
                builder);
        }
        #endregion

        #region SetNetworkAuthorization Methods
        public NetworkMessageConfigurationBuilder<TData> SetFilter(Func<ServiceProvider, NetworkMessageConfiguration, Boolean> filter)
        {
            this.Filter = filter;

            return this;
        }

        public NetworkMessageConfigurationBuilder<TData> SetPeerFilter<TPeer>()
            where TPeer : Peer
        {
            return this.SetFilter((p, _) => p.GetService<Peer>() is TPeer);
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
                this.Filter ?? DefaultFilter);
        }

        private static Boolean DefaultFilter(ServiceProvider providert, NetworkMessageConfiguration configuration)
        {
            return true;
        }
        #endregion
    }
}
