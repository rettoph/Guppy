using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using log4net;
using log4net.Core;
using Guppy.Extensions.log4net;
using log4net.Appender;
using Guppy.Example.Library.Scenes;
using Guppy.Example.Server.Scenes;

namespace Guppy.Example.Server.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ServerServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.RegisterTypeFactory<ExampleGame>(method: p => new ExampleServerGame(), priority: 1);
            services.RegisterTypeFactory<ExampleScene>(method: p => new ExampleServerScene(), priority: 1);

            services.RegisterSetup<NetPeerConfiguration>((config, p, c) =>
            {
                config.Port = 1337;
                config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
                config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            });

            services.RegisterSetup<ILog>((l, p, s) =>
            {
                l.SetLevel(Level.Verbose);
                l.ConfigureManagedColoredConsoleAppender(new ManagedColoredConsoleAppender.LevelColors()
                    {
                        BackColor = ConsoleColor.Red,
                        ForeColor = ConsoleColor.White,
                        Level = Level.Fatal
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Red,
                        Level = Level.Error
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Yellow,
                        Level = Level.Warn
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.White,
                        Level = Level.Info
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Magenta,
                        Level = Level.Debug
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Cyan,
                        Level = Level.Verbose
                    });
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
