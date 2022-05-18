using Guppy.Loaders;
using Guppy.Network.Definitions.Setups;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders
{
    internal sealed class SetupsServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSetup<NetScopeSetupDefinition>();
            services.AddSetup<NetTargetSetupDefinition>();
        }
    }
}
