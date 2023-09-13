using Autofac;
using Guppy.Loaders;
using Guppy.MonoGame.Serialization.Json.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class JsonLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<ColorConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<Vector2Converter>().As<JsonConverter>().SingleInstance();
        }
    }
}
