using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Game.Serialization.Json.Converters;
using Guppy.Engine.Loaders;
using System.Text.Json.Serialization;

namespace Guppy.Game.Loaders
{
    [AutoLoad]
    internal sealed class JsonLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<ColorConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<Vector2Converter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<Vector3Converter>().As<JsonConverter>().SingleInstance();
        }
    }
}
