using Guppy.Attributes;
using Guppy.Example.Library;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.CommandLine;
using System.CommandLine.Invocation;
using Guppy.CommandLine;
using Guppy.CommandLine.Services;
using System.CommandLine.IO;
using log4net;
using Guppy.IO.Extensions.log4net;
using log4net.Core;
using Microsoft.Xna.Framework;
using Guppy.CommandLine.Extensions.DependencyInjection;
using Minnow.General.Interfaces;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.Examples.Client.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ClientServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assmelbyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<ExampleGame>()
                .SetDefaultConstructor<ExampleClientGame>()
                .SetPriority(1);

            services.RegisterSetup<CommandService>()
                .SetMethod((commands, _, _) =>
                {
                    var hello = new Command("hello", "This is the base hello command")
                    {
                    };

                    var world = new Command("world", "Hello World!")
                    {
                    };

                    var dolly = new Command("dolly", "Hello, Dolly.")
                    {
                    };

                    hello.AddCommand(world);
                    hello.AddCommand(dolly);

                    commands.Get().Add(hello);
                    commands.Get().Add(new Command("test", "This is a test command")
                    {
                        new Argument<string>("input", "This is a test argument"),
                    });
                });

            services.RegisterSetup<ILog>()
                .SetMethod((l, p, _) =>
                {
                    l.ConfigureTerminalAppender(
                        p,
                        (Level.Fatal, Color.Red),
                        (Level.Error, Color.Red),
                        (Level.Warn, Color.Yellow),
                        (Level.Info, Color.White),
                        (Level.Debug, Color.Magenta),
                        (Level.Verbose, Color.Cyan)
                    );
                });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();

            // provider.GetCommand("test").Handler = CommandHandler.Create<String, IConsole>((input, console) =>
            // {
            //     console.Out.WriteLine($"Your custom input: {input}");
            // });
            // 
            // provider.GetCommand("hello world").Handler = CommandHandler.Create<IConsole>((console) =>
            // {
            //     console.Out.WriteLine($"The Earth says hello!");
            // });
            // 
            // provider.GetCommand("hello dolly").Handler = CommandHandler.Create<IConsole>((console) =>
            // {
            //     console.Out.WriteLine($"Hello, Dolly,");
            //     console.Out.WriteLine($"Well, hello, Dolly");
            //     console.Out.WriteLine($"It's so nice to have you back where you belong");
            // });
        }
    }
}
