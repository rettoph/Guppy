using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.log4net;
using Guppy.IO.Extensions.log4net;
using Guppy.Interfaces;
using Lidgren.Network;
using log4net;
using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Example.Library.Scenes;

namespace Guppy.Example.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ExampleServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<ExampleGame>(p => new ExampleGame());
            services.RegisterTypeFactory<ExampleScene>(p => new ExampleScene());
            services.RegisterTypeFactory<Ball>(p => new Ball());

            services.RegisterGame<ExampleGame>();
            services.RegisterScene<ExampleScene>();
            services.RegisterService<Ball>();

            services.RegisterSetup<ILog>((l, p, s) =>
            {
                l.SetLevel(Level.Verbose);
                l.ConfigureFileAppender($"logs\\{DateTime.Now.ToString("yyy-MM-dd")}.txt");
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
