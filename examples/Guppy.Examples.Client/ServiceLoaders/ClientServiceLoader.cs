using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Examples.Client.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ClientServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<ExampleGame>(factory: p => new ExampleClientGame(), priority: 1);

            services.AddSetup<NetPeerConfiguration>((config, p, c) =>
            {
                config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
