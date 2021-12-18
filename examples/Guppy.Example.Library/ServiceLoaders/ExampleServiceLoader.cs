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

namespace Guppy.Example.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ExampleServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<ExampleGame>().SetDefaultConstructor<ExampleGame>();
            services.RegisterTypeFactory<ExampleScene>().SetDefaultConstructor<ExampleScene>();
            services.RegisterTypeFactory<Ball>().SetDefaultConstructor<Ball>();

            services.RegisterGame<ExampleGame>();
            services.RegisterScene<ExampleScene>();
            services.RegisterService<Ball>();

            services.RegisterSetup<ILog>()
                .SetMethod((l, p, s) =>
                {
                    l.SetLevel(Level.Verbose);
                    l.ConfigureFileAppender($"logs\\{DateTime.Now.ToString("yyy-MM-dd")}.txt");
                });
        }
    }
}
