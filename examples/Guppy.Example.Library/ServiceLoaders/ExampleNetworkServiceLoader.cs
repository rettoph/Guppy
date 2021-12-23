using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Example.Library.Messages;
using Guppy.Network.Builders;
using Guppy.Network.Interfaces;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ExampleNetworkServiceLoader : INetworkLoader
    {
        public void ConfigureNetwork(NetworkProviderBuilder network)
        {
            network.SequenceChannelCount = 2;

            network.RegisterDataType<PositionDto>()
                .SetReader(PositionDto.Read)
                .SetWriter(PositionDto.Write);

            network.RegisterDataType<BallRadiusDto>()
                .SetReader(BallRadiusDto.Read)
                .SetWriter(BallRadiusDto.Write);

            network.RegisterNetworkEntityMessage<PositionMessage>()
                .SetDeliveryMethod(DeliveryMethod.Sequenced)
                .SetSequenceChannel(1);
        }
    }
}
