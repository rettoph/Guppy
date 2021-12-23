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
        public readonly DynamicId Id;

        public readonly DataConfiguration DataConfiguration;
        public readonly DeliveryMethod DeliveryMethod;
        public readonly Byte SequenceChannel;

        public readonly String ProcessorConfigurationName;
        public readonly Func<ServiceProvider, NetworkMessageConfiguration, Boolean> Filter;

        internal NetworkMessageConfiguration(
            DynamicId id,
            DataConfiguration dataConfiguration,
            DeliveryMethod deliveryMethod,
            Byte sequenceChannel,
            String processorConfigurationName,
            Func<ServiceProvider, NetworkMessageConfiguration, Boolean> filter)
        {
            this.Id = id;
            this.DataConfiguration = dataConfiguration;
            this.DeliveryMethod = deliveryMethod;
            this.SequenceChannel = sequenceChannel;
            this.ProcessorConfigurationName = processorConfigurationName;
            this.Filter = filter;
        }

        internal abstract void TryRegisterProcessor(ServiceProvider provider, MessageQueue<IData> messages);


        internal MessageConfigurationMessage ToMessage()
        {
            return new MessageConfigurationMessage()
            {
                Id = this.Id.Value,
                DataTypeId = this.DataConfiguration.Id.Value
            };
        }
    }

    internal class NetworkMessageConfiguration<TData> : NetworkMessageConfiguration
        where TData : class, IData
    {
        public NetworkMessageConfiguration(DynamicId id, DataConfiguration dataConfiguration, DeliveryMethod deliveryMethod, byte sequenceChannel, String processorConfigurationName, Func<ServiceProvider, NetworkMessageConfiguration, bool> filter) : base(id, dataConfiguration, deliveryMethod, sequenceChannel, processorConfigurationName, filter)
        {
        }

        internal override void TryRegisterProcessor(ServiceProvider provider, MessageQueue<IData> messages)
        {
            if(this.ProcessorConfigurationName is null)
            {
                return;
            }

            messages.RegisterProcessor<TData>(provider.GetService<IMessageProcessor<TData>>(this.ProcessorConfigurationName));
        }
    }
}
