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
        public readonly NetworkAuthorization SenderAuthorization;
        public readonly Int32 IncomingPriority;
        public readonly Int32 OutgoingPriority;

        protected NetworkMessageConfiguration(
            Type type,
            DynamicId id,
            DataConfiguration dataConfiguration,
            DeliveryMethod deliveryMethod,
            Byte sequenceChannel,
            Type processorConfigurationType,
            NetworkAuthorization senderAuthorization,
            Int32 incomingPriority,
            Int32 outgoingPriority)
        {
            this.Type = type;
            this.Id = id;
            this.DataConfiguration = dataConfiguration;
            this.DeliveryMethod = deliveryMethod;
            this.SequenceChannel = sequenceChannel;
            this.ProcessorConfigurationType = processorConfigurationType;
            this.SenderAuthorization = senderAuthorization;
            this.IncomingPriority = incomingPriority;
            this.OutgoingPriority = outgoingPriority;
        }

        internal abstract void TryRegisterIncomingProcessor(ServiceProvider provider, MessageBus bus);

        public Boolean CanSend(NetworkAuthorization authorization)
        {
            return (this.SenderAuthorization & authorization) != 0;
        }

        public Boolean CanRecieve(NetworkAuthorization authorization)
        {
            return this.SenderAuthorization != authorization;
        }
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
            NetworkAuthorization senderAuthorization,
            Int32 incomingPriority,
            Int32 outgoingPriority) : base(typeof(TMessage), id, dataConfiguration, deliveryMethod, sequenceChannel, processorConfigurationType, senderAuthorization, incomingPriority, outgoingPriority)
        {
        }

        internal override void TryRegisterIncomingProcessor(ServiceProvider provider, MessageBus bus)
        {
            if(this.ProcessorConfigurationType is null)
            {
                return;
            }

            bus.RegisterProcessor<TMessage>(provider.GetService<IDataProcessor<TMessage>>(this.ProcessorConfigurationType));
        }
    }
}
