using Guppy.Attributes;
using Guppy.Attributes.Common;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Network.Definitions;
using Microsoft.Extensions.DependencyInjection;
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
            var serializers = assemblies.GetAttributes<INetSerializer, AutoLoadAttribute>(true).Types;

            foreach (Type serializer in serializers)
            {
                services.AddNetSerializer(serializer);
            }

            var messageTypes = assemblies.GetAttributes<NetMessageTypeDefinition, AutoLoadAttribute>(true).Types;

            foreach (Type messenger in messageTypes)
            {
                services.AddNetMessageType(messenger);
            }
        }
    }
}
