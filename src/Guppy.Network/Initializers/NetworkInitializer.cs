using Guppy.Initializers;
using Guppy.Network.Initializers.Collections;
using Guppy.Network.Loaders;
using Guppy.Network.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Initializers
{
    internal sealed class NetworkInitializer : GuppyInitializer<INetworkLoader>
    {
        protected override void Initialize(AssemblyHelper assemblies, IServiceCollection services, IEnumerable<INetworkLoader> loaders)
        {
            var serializersCollection = new NetSerializerCollection();
            var messengersCollection = new NetMessengerCollection();

            foreach (INetworkLoader loader in loaders)
            {
                loader.ConfigureNetSerializers(serializersCollection);
            }

            foreach (INetworkLoader loader in loaders)
            {
                loader.ConfigureNetMessengers(messengersCollection);
            }

            var serializers = serializersCollection.BuildNetSerializerProvider();
            var messengers = messengersCollection.BuildNetMessengerProvider(serializers);

            services.AddSingleton<INetSerializerProvider>(serializers);
            services.AddSingleton<INetMessengerProvider>(messengers);
        }
    }
}
