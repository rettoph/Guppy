using Guppy.Resources.Definitions;

namespace Autofac
{
    public static partial class ContainerBuilderExtensions
    {
        public static void AddSetting<T>(this ContainerBuilder services)
            where T : class, ISettingDefinition
        {
            services.RegisterType<T>().As<ISettingDefinition>().SingleInstance();
        }

        public static void AddSetting(this ContainerBuilder services, Type definitionType)
        {
            services.RegisterType(definitionType).As<ISettingDefinition>().SingleInstance();
        }

        public static void AddSetting<T>(this ContainerBuilder services, string key, T defaultValue, bool exportable, params string[] tags)
            where T : notnull
        {
            services.RegisterInstance(new RuntimeSettingDefinition<T>(key, defaultValue, exportable, tags)).As<ISettingDefinition>().SingleInstance();
        }

        public static void AddSetting<T>(this ContainerBuilder services, T defaultValue, bool exportable, params string[] tags)
            where T : notnull
        {
            services.RegisterInstance(new RuntimeSettingDefinition<T>(typeof(T).FullName ?? throw new Exception(), defaultValue, exportable, tags)).As<ISettingDefinition>().SingleInstance();
        }
    }
}
