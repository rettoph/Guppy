using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Loaders;
using Guppy.Engine.Common.Services;
using Guppy.Engine.Services;
using System.Text.Json.Serialization;

namespace Guppy.Engine.Loaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<ObjectTextFilterService>().As<IObjectTextFilterService>().SingleInstance();

            services.RegisterInstance(new JsonStringEnumConverter()).As<JsonConverter>().SingleInstance();
        }
    }
}
