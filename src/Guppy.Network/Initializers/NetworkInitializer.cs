using Guppy.Attributes;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Network.Definitions;
using Guppy.Network.Loaders;
using Guppy.Network.Providers;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Initializers
{
    internal sealed class NetworkInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var serializers = assemblies.GetAttributes<NetSerializerDefinition, AutoLoadAttribute>().Types;

            foreach (Type serializer in serializers)
            {
                services.AddNetSerializer(serializer);
            }

            var messengers = assemblies.GetAttributes<NetMessengerDefinition, AutoLoadAttribute>().Types;

            foreach (Type messenger in messengers)
            {
                services.AddNetMessenger(messenger);
            }

            services.AddSingleton<INetSerializerProvider, NetSerializerProvider>();
            services.AddSingleton<INetMessengerProvider, NetMessengerProvider>();
        }
    }
}
