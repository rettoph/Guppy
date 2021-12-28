using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Example.Library.Messages;
using Guppy.Network.Builders;
using Guppy.Network.Interfaces;
using Guppy.ServiceLoaders;
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
            network.SequenceChannelCount = 3;

            network.RegisterDataType<PositionDto>()
                .SetReader(PositionDto.Read)
                .SetWriter(PositionDto.Write);

            network.RegisterDataType<UserDto>()
                .SetReader(UserDto.Read)
                .SetWriter(UserDto.Write);

            network.RegisterDataType<PaddleTargetDto>()
                .SetReader(PaddleTargetDto.Read)
                .SetWriter(PaddleTargetDto.Write);

            network.RegisterDataType<GoalZoneDto>()
                .SetReader(GoalZoneDto.Read)
                .SetWriter(GoalZoneDto.Write);


            network.RegisterNetworkEntityMessage<PositionMessage>()
                .SetDeliveryMethod(DeliveryMethod.Sequenced)
                .SetSequenceChannel(1);

            network.RegisterNetworkEntityMessage<PaddleTargetRequestMessage>()
                .SetDeliveryMethod(DeliveryMethod.Sequenced)
                .SetSequenceChannel(2);

            network.RegisterNetworkEntityMessage<PaddleTargetMessage>()
                .SetDeliveryMethod(DeliveryMethod.Sequenced)
                .SetSequenceChannel(2);
        }
    }
}
