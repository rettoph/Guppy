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
        public class MyExample { 
        }

        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            #region Register Services
            services.RegisterService<EventBasedNetListener>()
                .SetLifetime(ServiceLifetime.Singleton)
                .CreateTypeFactory(factory =>
                {
                    factory.SetMethod((_, _) => new EventBasedNetListener());
                });

            services.RegisterService<NetManager>()
                .SetLifetime(ServiceLifetime.Singleton)
                .CreateTypeFactory(factory =>
                {
                    factory.SetMethod((p, _) => new NetManager(p.GetService<EventBasedNetListener>()));
                });
            #endregion
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
