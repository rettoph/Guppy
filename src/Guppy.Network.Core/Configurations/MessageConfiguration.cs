using Guppy.EntityComponent.DependencyInjection;
using Minnow.General.Interfaces;
using Guppy.Network.Dtos;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.MessageProcessors;
using Guppy.Network.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public class MessageConfiguration
    {
        public readonly DynamicId Id;

        public readonly String Name;

        public readonly DataTypeConfiguration DataType;
        public readonly DeliveryMethod DeliveryMethod;
        public readonly Byte SequenceChannel;

        public readonly Func<ServiceProvider, MessageProcessor> ProcessorFactory;

        public readonly Func<ServiceProvider, MessageConfiguration, Boolean> Filter;

        internal MessageConfiguration(
            DynamicId id,
            String name,
            DataTypeConfiguration dataType,
            DeliveryMethod deliveryMethod,
            Byte sequenceChannel,
            Func<ServiceProvider, MessageProcessor> messageHandlerFactory,
            Func<ServiceProvider, MessageConfiguration, Boolean> filter)
        {
            this.Id = id;
            this.Name = name;
            this.DataType = dataType;
            this.DeliveryMethod = deliveryMethod;
            this.SequenceChannel = sequenceChannel;
            this.ProcessorFactory = messageHandlerFactory;
            this.Filter = filter;
        }

        internal MessageConfigurationDto ToDto()
        {
            return new MessageConfigurationDto()
            {
                Id = this.Id.Value,
                Name = this.Name,
                DataTypeId = this.DataType.Id.Value
            };
        }
    }
}
