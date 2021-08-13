using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Components.Lists;
using Guppy.Network.Components.Scenes;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Peers;
using Guppy.Network.Pipes;
using Guppy.Network.Scenes;
using Guppy.Network.Services;
using Guppy.Network.Utilities;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void RegisterServices(GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<Broadcasts>(_ => new Broadcasts());
            services.RegisterTypeFactory<Broadcast>(_ => new Broadcast());

            services.RegisterScoped<Broadcasts>();
            services.RegisterTransient<Broadcast>();

            #region Components

            services.RegisterTypeFactory<NetworkServiceListLayerableComponent>(p => new NetworkServiceListLayerableComponent());
            services.RegisterTypeFactory<NetworkSceneMasterCRUDComponent>(p => new NetworkSceneMasterCRUDComponent());
            services.RegisterTypeFactory<NetworkSceneSlaveCRUDComponent>(p => new NetworkSceneSlaveCRUDComponent());
            services.RegisterTypeFactory<NetworkEntityCRUDComponent>(p => new NetworkEntityCRUDComponent());

            services.RegisterTransient<NetworkServiceListLayerableComponent>();
            services.RegisterTransient<NetworkSceneMasterCRUDComponent>();
            services.RegisterTransient<NetworkSceneSlaveCRUDComponent>();
            services.RegisterTransient<NetworkEntityCRUDComponent>();

            services.RegisterComponent<NetworkServiceListLayerableComponent, NetworkEntityList>();
            services.RegisterComponent<NetworkSceneMasterCRUDComponent, NetworkScene>();
            services.RegisterComponent<NetworkSceneSlaveCRUDComponent, NetworkScene>();
            services.RegisterComponent<NetworkEntityCRUDComponent, INetworkEntity>();
            #endregion
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
