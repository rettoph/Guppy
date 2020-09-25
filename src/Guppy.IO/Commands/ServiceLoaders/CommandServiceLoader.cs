using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.ServiceLoaders
{
    [AutoLoad]
    internal sealed class CommandServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddFactory<CommandService>(p => new CommandService());
            services.AddSingleton<CommandService>(autoBuild: true);

            services.AddConfiguration<CommandService>((c, p, d) =>
            {
                c.Add(new HelloWorldContext());

                c.Excecute("helloworld -test true");
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
