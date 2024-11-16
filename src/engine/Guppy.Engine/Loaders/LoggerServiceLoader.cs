using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Loaders;
using Guppy.Engine.Common.Providers;
using Guppy.Engine.Providers;

namespace Guppy.Engine.Loaders
{
    [AutoLoad]
    internal sealed class LoggerServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<LogLevelProvider>().As<ILogLevelProvider>().SingleInstance();
        }
    }
}
