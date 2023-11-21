using Autofac;
using Guppy.Common;
using Guppy.Loaders;
using Guppy.Resources.Constants;
using Guppy.Resources.Providers;
using Guppy.Serialization;
using System.Text.Json.Serialization;
using STJ = System.Text.Json;

namespace Guppy.Resources.Loaders
{
    internal sealed class SettingLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<SettingProvider>().As<ISettingProvider>().SingleInstance();
            services.RegisterInstance(new JsonStringEnumConverter(STJ.JsonNamingPolicy.CamelCase)).As<JsonConverter>().SingleInstance();

            services.AddSetting(SettingConstants.Localization, Localization.Default, true, nameof(Guppy));
        }
    }
}
