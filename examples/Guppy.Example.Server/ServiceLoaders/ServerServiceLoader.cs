using Guppy.Attributes;
using Guppy.Example.Library;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Core;
using log4net.Appender;
using Guppy.Example.Library.Scenes;
using Guppy.Example.Server.Scenes;
using Minnow.General.Interfaces;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.ServiceLoaders;

namespace Guppy.Example.Server.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ServerServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<ExampleGame>()
                .SetDefaultConstructor<ServerExampleGame>()
                .SetPriority(1);

            services.RegisterTypeFactory<ExampleScene>()
                .SetDefaultConstructor<ServerExampleScene>()
                .SetPriority(1);

            services.RegisterSetup<ILog>()
                .SetMethod((l, p, s) =>
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
    }
}
