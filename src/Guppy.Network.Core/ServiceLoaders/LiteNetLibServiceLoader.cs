using Guppy.EntityComponent.DependencyInjection;
using Guppy.Attributes;
using Guppy.Interfaces;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.ServiceLoaders;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LiteNetLibServiceLoader : IServiceLoader
    {
        public class MyExample { 
        }

        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            #region Register Services
            services.RegisterService<EventBasedNetListener>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<EventBasedNetListener>();
                });

            services.RegisterService<NetManager>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => new NetManager(p.GetService<EventBasedNetListener>())
                    {
                        ChannelsCount = p.GetService<NetworkProvider>().SequenceChannelCount
                    });
                });
            #endregion
        }
    }
}
