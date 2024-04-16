using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Game.Commands.Common.Services;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Common.Loaders;
using Guppy.Core.Messaging.Common;
using System.CommandLine;
using System.CommandLine.IO;

namespace Guppy.Game.Commands.Common.Loaders
{
    [AutoLoad]
    internal sealed class CommandLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<CommandService>().As<ICommandService>().As<IMagicBroker>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);
            services.RegisterType<SystemConsole>().As<IConsole>().SingleInstance();
        }
    }
}
