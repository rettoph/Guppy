using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.log4net;
using Guppy.Interfaces;
using Lidgren.Network;
using log4net;
using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class ExampleServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<ExampleGame>(p => new ExampleGame());

            services.AddSingleton<ExampleGame>();

            services.AddSetup<ILog>((l, p, s) =>
            {
                l.SetLevel(Level.Verbose);
                l.ConfigureFileAppender($"logs\\{DateTime.Now.ToString("yyy-MM-dd")}.txt")
                    .ConfigureManagedColoredConsoleAppender(new ManagedColoredConsoleAppender.LevelColors()
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
