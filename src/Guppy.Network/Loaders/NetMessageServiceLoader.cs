using Guppy.Loaders;
using Guppy.Network.Definitions.NetMessageFactories;
using Guppy.Network.Definitions.NetSerializers;
using Guppy.Network.Security.Definitions.NetMessengers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    internal sealed class NetMessageServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNetSerializer<UserActionMessageNetSerializerDefinition>();
            services.AddNetSerializer<NetTargetActionMessageNetSerializerDefinition>();

            services.AddNetMessenger<UserActionMessageNetMessageFactoryDefinition>();
            services.AddNetMessenger<NetTargetActionMessageNetMessageFactoryDefinition>();
        }
    }
}
