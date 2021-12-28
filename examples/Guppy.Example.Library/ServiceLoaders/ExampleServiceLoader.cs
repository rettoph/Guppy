using Guppy.Attributes;
using Guppy.Extensions.log4net;
using Guppy.IO.Extensions.log4net;
using Guppy.Interfaces;
using log4net;
using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Example.Library.Scenes;
using Guppy.Network;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Entities;
using Guppy.Example.Library.Components.Entities;
using Guppy.ServiceLoaders;

namespace Guppy.Example.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ExampleServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterGame<ExampleGame>();
            services.RegisterScene<ExampleScene>();

            services.RegisterService<Ball>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Ball>();
                });

            services.RegisterService<GoalZone>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<GoalZone>();
                });

            services.RegisterService<List<Paddle>>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<List<Paddle>>();
                });

            services.RegisterSetup<ILog>()
                .SetMethod((l, p, s) =>
                {
                    l.SetLevel(Level.Verbose);
                    l.ConfigureFileAppender($"logs\\{DateTime.Now.ToString("yyy-MM-dd")}.txt");
                });

            #region Entities
            services.RegisterEntity<Paddle>()
                .RegisterService(service =>
                {
                    service.SetLifetime(ServiceLifetime.Transient)
                        .RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<Paddle>();
                        });
                })
                .RegisterComponent<PaddleRemoteSlaveComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<PaddleRemoteSlaveComponent>();
                        });
                    });
                })
                .RegisterComponent<PaddleRemoteMasterComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<PaddleRemoteMasterComponent>();
                        });
                    });
                });

            services.RegisterEntity<Positionable>()
                .RegisterComponent<PositionableRemoteSlaveComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<PositionableRemoteSlaveComponent>();
                        });
                    });
                })
                .RegisterComponent<PositionableRemoteMasterComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<PositionableRemoteMasterComponent>();
                        });
                    });
                });
            #endregion
        }
    }
}