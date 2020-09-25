using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Commands.Demos;
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
            services.AddFactory<Segment>(p => new Segment());

            services.AddSingleton<CommandService>(autoBuild: true);
            services.AddTransient<Segment>();

            services.AddConfiguration<CommandService>((commands, p, d) =>
            {
                commands.TryAdd(new SegmentContext()
                {
                    Identifier = "hello",
                    SubSegments = new SegmentContext[] {
                        new SegmentContext()
                        {
                            Identifier = "world",
                            Command = new WorldCommand()
                        },
                        new SegmentContext()
                        {
                            Identifier = "dolly"
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
