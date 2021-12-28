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
using Guppy.CommandLine.Interfaces;
using Guppy.CommandLine.Builders;
using Guppy.Example.Library.Scenes;
using Guppy.Example.Client.Scenes;
using Guppy.Example.Client.Components.Entities;
using Guppy.Example.Library.Entities;
using Guppy.ServiceLoaders;

namespace Guppy.Example.Client.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ClientServiceLoader : IServiceLoader, ICommandLoader
    {
        public void RegisterServices(AssemblyHelper assmelbyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<ExampleGame>()
                .SetDefaultConstructor<ClientExampleGame>()
                .SetPriority(1);

            services.RegisterTypeFactory<ExampleScene>()
                .SetDefaultConstructor<ClientExampleScene>()
                .SetPriority(1);

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


            services.RegisterEntity<Paddle>()
                .RegisterComponent<PaddleDrawComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<PaddleDrawComponent>();
                        });
                    });
                })
                .RegisterComponent<PaddleCurrentUserComponent>(component =>
                {
                    component.RegisterService(service =>
                    {
                        service.RegisterTypeFactory(factory =>
                        {
                            factory.SetDefaultConstructor<PaddleCurrentUserComponent>();
                        });
                    });
                });

            services.RegisterComponent<BallDrawComponent>()
                .SetAssignableEntityType<Ball>()
                .RegisterService(service =>
                {
                    service.RegisterTypeFactory(factory =>
                    {
                        factory.SetDefaultConstructor<BallDrawComponent>();
                    });
                });

            services.RegisterComponent<GoalZoneDrawComponent>()
                .SetAssignableEntityType<GoalZone>()
                .RegisterService(service =>
                {
                    service.RegisterTypeFactory(factory =>
                    {
                        factory.SetDefaultConstructor<GoalZoneDrawComponent>();
                    });
                });
        }

        public void RegisterCommands(CommandServiceBuilder commands)
        {
            // commands.RegisterCommand("test")
            //     .SetDescription("This is a test command")
            //     .AddArgument(new Argument<string>("input", "This is a test argument"))
            //     .SetDefaultHandler(CommandHandler.Create<String, IConsole>((input, console) =>
            //     {
            //         console.Out.WriteLine($"Your custom input: {input}");
            //     }));
            // 
            // commands.RegisterCommand("hello")
            //     .SetDescription("This is the base hello command")
            //     .AddSubCommand("world", world =>
            //     {
            //         world.SetDescription("Hello World!")
            //             .SetDefaultHandler(CommandHandler.Create<IConsole>((console) =>
            //             {
            //                 console.Out.WriteLine($"The Earth says hello!");
            //             }));
            //     })
            //     .AddSubCommand("dolly", dolly =>
            //     {
            //         dolly.SetDescription("Hello, Dolly.")
            //             .SetDefaultHandler(CommandHandler.Create<IConsole>((console) =>
            //             {
            //                 console.Out.WriteLine($"Hello, Dolly,");
            //                 console.Out.WriteLine($"Well, hello, Dolly");
            //                 console.Out.WriteLine($"It's so nice to have you back where you belong");
            //             }));
            //     });
        }
    }
}
