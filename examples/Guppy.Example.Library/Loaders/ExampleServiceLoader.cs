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

namespace Guppy.Example.Library.Loaders
{
    [AutoLoad]
    internal sealed class ExampleServiceLoader : IServiceLoader, IComponentLoader, INetworkLoader
    {
        public void ConfigureNetSerializers(INetSerializerCollection serializers)
        {
            serializers.Add<TestNetMessage>(TestNetMessage.Serialize, TestNetMessage.Deserialize);
        }

        public void ConfigureNetMessengers(INetMessengerCollection messengers)
        {
            messengers.Add<TestNetMessage>(DeliveryMethod.ReliableOrdered, 0, 0);
        }

        public void ConfigureComponents(IComponentCollection components)
        {
            components.Add<Scene, TestComponent>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddActivated<Scene, ExampleScene>();
        }
    }
}
