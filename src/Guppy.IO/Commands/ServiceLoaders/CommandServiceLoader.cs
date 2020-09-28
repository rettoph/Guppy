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
            services.AddFactory<Command>(p => new Command());

            services.AddSingleton<CommandService>(autoBuild: true);
            services.AddTransient<Command>();

            services.AddConfiguration<CommandService>((commands, p, d) =>
            {
                commands.TryAddSubCommand(new CommandContext()
                {
                    Word = "hello",
                    SubCommands = new CommandContext[] {
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
