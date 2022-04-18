using Guppy.Loaders;
using Guppy.Network.Constants;
using Guppy.Network.Definitions.NetSerializers;
using Guppy.Network.Loaders;
using Guppy.Network.Security.Definitions.NetMessengers;
using Guppy.Network.Security.Definitions.NetSerializers;
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
    internal sealed class SecurityLoader : IServiceLoader, IBusLoader
    {
        public void ConfigureBus(IBusMessageCollection bus)
        {
            bus.AddNetIncomingMessage<ConnectionResponseMessage>(BusConstants.PeerQueuePriority);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();

            services.AddNetSerializer<ConnectionResponseMessageNetSerializerDefinition>();

            services.AddNetMessenger<ConnectionResponseMessageNetMessengerDefinition>();
        }
    }
}
