using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.Example.Library.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Loaders;
using Guppy.EntityComponent.Loaders;
using Guppy.Network.Loaders;
using Guppy.Network;
using LiteNetLib;
using Guppy.Gaming;
using Guppy.Threading.Loaders;
using Guppy.Threading;
using Guppy.Example.Library.Definitions.NetSerializers;

namespace Guppy.Example.Library.Loaders
{
    [AutoLoad]
    internal sealed class ExampleServiceLoader : IServiceLoader, IBusLoader
    {
        public void ConfigureBus(IBusMessageCollection bus)
        {
            bus.AddNetDeserialized<TestNetMessage>(0);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScene<ExampleScene>();

            services.AddComponent<Scene, TestComponent>();
        }
    }
}
