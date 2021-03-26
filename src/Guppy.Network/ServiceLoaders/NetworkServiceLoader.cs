using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Network.Drivers;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<ServiceList<NetworkEntity>>(p => new ServiceList<NetworkEntity>());
            services.AddScoped<ServiceList<NetworkEntity>>();


            #region Settings Setup
            services.AddSetup<Settings>((s, p, c) =>
            { // Configure the default settings...
                s.Set<NetworkAuthorization>(NetworkAuthorization.Slave);
                s.Set<HostType>(HostType.Remote);
            }, -10);

            services.AddSetup<ServerPeer>((server, p, c) =>
            {
                p.Settings.Set<NetworkAuthorization>(NetworkAuthorization.Master);
            });
            #endregion

            #region Default Driver Filters
            services.AddDriverFilter(
                typeof(MasterNetworkAuthorizationDriver<>),
                (d, p) => p.Settings.Get<NetworkAuthorization>() == NetworkAuthorization.Master);

            services.AddDriverFilter(
                typeof(SlaveNetworkAuthorizationDriver<>),
                (d, p) => p.Settings.Get<NetworkAuthorization>() == NetworkAuthorization.Slave);
            #endregion
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
