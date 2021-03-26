using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Server.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ServerServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<ExampleGame>(factory: p => new ExampleServerGame(), priority: 1);

            services.AddSetup<NetPeerConfiguration>((config, p, c) =>
            {
                config.Port = 1337;
                config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
                config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
