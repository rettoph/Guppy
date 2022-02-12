using Guppy.EntityComponent.DependencyInjection;
using Minnow.General.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.MessageProcessors;
using Guppy.Network.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;

namespace Guppy.Network.Configurations
{
    public abstract class NetworkMessageConfiguration
    {
        public readonly Type Type;

        public readonly DynamicId Id;

        public readonly DataConfiguration DataConfiguration;
        public readonly DeliveryMethod DeliveryMethod;
        public readonly Byte SequenceChannel;

        public readonly Type ProcessorConfigurationType;
        public readonly Func<ServiceProvider, NetworkMessageConfiguration, Boolean> Filter;
        public readonly Int32 MessageBusQueue;

        protected NetworkMessageConfiguration(
            Type type,
            DynamicId id,
            DataConfiguration dataConfiguration,
            DeliveryMethod deliveryMethod,
            Byte sequenceChannel,
            Type processorConfigurationType,
            Func<ServiceProvider, NetworkMessageConfiguration, Boolean> filter,
            Int32 messageBusQueue)
        {
            this.Type = type;
            this.Id = id;
            this.DataConfiguration = dataConfiguration;
            this.DeliveryMethod = deliveryMethod;
            this.SequenceChannel = sequenceChannel;
            this.ProcessorConfigurationType = processorConfigurationType;
            this.Filter = filter;
            this.MessageBusQueue = messageBusQueue;
        }

        internal abstract void TryRegisterProcessor(ServiceProvider provider, MessageBus bus);
    }

    internal class NetworkMessageConfiguration<TMessage> : NetworkMessageConfiguration
        where TMessage : class, IMessage
    {
        public NetworkMessageConfiguration(
            DynamicId id, 
            DataConfiguration dataConfiguration, 
            DeliveryMethod deliveryMethod, 
            byte sequenceChannel, 
            Type processorConfigurationType, 
            Func<ServiceProvider, NetworkMessageConfiguration, bool> filter,
            Int32 messageBusQueue) : base(typeof(TMessage), id, dataConfiguration, deliveryMethod, sequenceChannel, processorConfigurationType, filter, messageBusQueue)
        {
        }

        internal override void TryRegisterProcessor(ServiceProvider provider, MessageBus bus)
        {
            if(this.ProcessorConfigurationType is null)
            {
                return;
            }

            bus.RegisterProcessor<TMessage>(provider.GetService<IDataProcessor<TMessage>>(this.ProcessorConfigurationType));
        }
    }
}
