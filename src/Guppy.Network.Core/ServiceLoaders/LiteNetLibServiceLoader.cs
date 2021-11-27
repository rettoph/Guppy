using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using LiteNetLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LiteNetLibServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            #region Register TypeFactories
            services.RegisterTypeFactory<EventBasedNetListener>(p => new EventBasedNetListener());
            services.RegisterTypeFactory<NetManager>(p => new NetManager(p.GetService<EventBasedNetListener>()));
            #endregion

            #region Register Services
            services.RegisterService<EventBasedNetListener>()
                .SetLifetime(ServiceLifetime.Singleton);

            services.RegisterService<NetManager>()
                .SetLifetime(ServiceLifetime.Singleton);
            #endregion
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
