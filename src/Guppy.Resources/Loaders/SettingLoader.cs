using Autofac;
using Guppy.Common;
using Guppy.Loaders;
using Guppy.Resources.Constants;
using Guppy.Resources.Providers;
using Guppy.Resources.Serialization.Json.Converters;
using Guppy.Resources.Serialization.Settings;
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
            services.RegisterInstance(new JsonStringEnumConverter()).As<JsonConverter>().SingleInstance();

            services.RegisterType<SettingValueDictionaryConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<DefaultSettingSerializer<bool>>().As<SettingSerializer>().SingleInstance();
            services.RegisterType<DefaultSettingSerializer<string>>().As<SettingSerializer>().SingleInstance();
            services.RegisterType<DefaultSettingSerializer<int>>().As<SettingSerializer>().SingleInstance();
            services.RegisterType<DefaultSettingSerializer<float>>().As<SettingSerializer>().SingleInstance();
        }
    }
}
