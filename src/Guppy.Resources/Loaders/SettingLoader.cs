using Autofac;
using Guppy.Common;
using Guppy.Loaders;
using Guppy.Resources.Constants;
using Guppy.Resources.Providers;
using Guppy.Resources.Serialization.Json;
using Guppy.Resources.Serialization.Json.Implementations;
using System.Text.Json.Serialization;
using STJ = System.Text.Json;

namespace Guppy.Resources.Loaders
{
    internal sealed class SettingLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<SettingProvider>().As<ISettingProvider>().SingleInstance();
            services.RegisterType<JsonSerializer>().As<IJsonSerializer>().SingleInstance();
            services.RegisterInstance(new JsonStringEnumConverter(STJ.JsonNamingPolicy.CamelCase)).As<JsonConverter>().SingleInstance();

            services.AddSetting(SettingConstants.Localization, Localization.Default, true, nameof(Guppy));
        }
    }
}
