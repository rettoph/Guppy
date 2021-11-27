using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Interfaces;
using Lidgren.Network;
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
using Guppy.DependencyInjection.ServiceConfigurations;
using DotNetUtils.General.Interfaces;
using Guppy.DependencyInjection.Interfaces;

namespace Guppy.Examples.Client.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ClientServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assmelbyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<ExampleGame>((p, t) => new ExampleClientGame())
                .SetPriority(1);

            services.RegisterSetup<NetPeerConfiguration>((config, p, c) =>
            {
                config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            });

            services.RegisterSetup((CommandService commands, GuppyServiceProvider p, IServiceConfiguration c) =>
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

            services.RegisterSetup<ILog>((l, p, s) =>
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

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();

            provider.GetCommand("test").Handler = CommandHandler.Create<String, IConsole>((input, console) =>
            {
                console.Out.WriteLine($"Your custom input: {input}");
            });

            provider.GetCommand("hello world").Handler = CommandHandler.Create<IConsole>((console) =>
            {
                console.Out.WriteLine($"The Earth says hello!");
            });

            provider.GetCommand("hello dolly").Handler = CommandHandler.Create<IConsole>((console) =>
            {
                console.Out.WriteLine($"Hello, Dolly,");
                console.Out.WriteLine($"Well, hello, Dolly");
                console.Out.WriteLine($"It's so nice to have you back where you belong");
            });
        }
    }
}
