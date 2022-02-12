using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Network.Configurations;
using Guppy.Network.MessageProcessors;
using Guppy.Network.Messages;
using Guppy.Network.Structs;
using Guppy.Network.Utilities;
using Minnow.General;
using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Builders
{
    public class NetworkEntityMessageConfigurationBuilder<TNetworkEntityMessage> : NetworkMessageConfigurationBuilder<TNetworkEntityMessage, NetworkEntityMessageConfigurationBuilder<TNetworkEntityMessage>>
        where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
    {
        private NetworkProviderBuilder _network;

        public NetworkEntityMessageConfigurationBuilder(NetworkProviderBuilder network, ServiceProviderBuilder services) : base(network, services)
        {
            this.network.RegisterDataType<TNetworkEntityMessage>()
                .SetPriority(-1)
                .SetReader(NetworkEntityMessageSerializer<TNetworkEntityMessage>.GetReader())
                .SetWriter(NetworkEntityMessageSerializer<TNetworkEntityMessage>.GetWriter());
        }

        public override NetworkMessageConfiguration Build(DynamicId id, DoubleDictionary<ushort, Type, DataConfiguration> dataTypeConfigurations)
        {
            if(this.ProcessorConfigurationType is null)
            { // No custom processor was defined, so we should generate a default one now...
                this.RegisterProcessorConfiguration<DefaultNetworkEntityMessageProcessor<TNetworkEntityMessage>>(processor =>
                {
                    processor.SetLifetime(ServiceLifetime.Scoped)
                        .RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<DefaultNetworkEntityMessageProcessor<TNetworkEntityMessage>>();
                        });
                });
            }

            return base.Build(id, dataTypeConfigurations);
        }
    }
}
