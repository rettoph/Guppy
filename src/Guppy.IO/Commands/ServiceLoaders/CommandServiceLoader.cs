using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.ServiceLoaders
{
    [AutoLoad]
    internal sealed class CommandServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<CommandService>(p => new CommandService());
            services.AddFactory<Command>(p => new Command());

            services.AddSingleton<CommandService>();
            services.AddTransient<Command>();

            services.AddSetup<CommandService>((commands, p, d) =>
            {
                commands.TryAddCommand(new CommandContext()
                {
                    Word = "hello",
                    Commands = new CommandContext[] {
                        new CommandContext()
                        {
                            Word = "world",
                        },
                        new CommandContext()
                        {
                            Word = "dolly"
                        }
                    }
                });
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
