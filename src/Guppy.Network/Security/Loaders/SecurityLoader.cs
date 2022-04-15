using Guppy.Loaders;
using Guppy.Network.Constants;
using Guppy.Network.Loaders;
using Guppy.Network.Loaders.Collections;
using Guppy.Network.Security.Messages;
using Guppy.Network.Security.Services;
using Guppy.Threading;
using Guppy.Threading.Loaders;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Loaders
{
    internal sealed class SecurityLoader : INetworkLoader, IServiceLoader, IBusLoader
    {
        public void ConfigureBus(IBusMessageCollection bus)
        {
            bus.AddNetIncomingMessage<ConnectionResponseMessage>(BusConstants.PeerQueuePriority);
        }

        public void ConfigureNetMessengers(INetMessengerCollection messengers)
        {
            messengers.Add<ConnectionResponseMessage>(
                MessengerConstants.PeerDeliveryMethod,
                MessengerConstants.PeerOutgoingChannel,
                MessengerConstants.PeerOutgoingPriority);
        }

        public void ConfigureNetSerializers(INetSerializerCollection serializers)
        {
            serializers.Add<ConnectionResponseMessage>(
                ConnectionResponseMessage.Serialize,
                ConnectionResponseMessage.Deserialize);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
        }
    }
}
