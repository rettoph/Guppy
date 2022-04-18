using Guppy;
using Guppy.Definitions;
using Guppy.Definitions.Settings;
using Guppy.Definitions.SettingSerializers;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSetting<T>(this IServiceCollection services, string key, string name, string description, T defaultValue, bool exportable, params string[] tags)
        {
            var descriptor = new RuntimeSettingDefinition<T>(key, name, description, defaultValue, exportable, tags);
            return services.AddSetting(descriptor);
        }

        public static IServiceCollection AddSetting<T>(this IServiceCollection services, string name, string description, T defaultValue, bool exportable, params string[] tags)
        {
            var descriptor = new RuntimeSettingDefinition<T>(SettingDefinition.GetKey<T>(null), name, description, defaultValue, exportable, tags);
            return services.AddSetting(descriptor);
        }

        public static IServiceCollection AddSetting(this IServiceCollection services, SettingDefinition definition)
        {
            return services.AddSingleton<SettingDefinition>(definition);
        }

        public static IServiceCollection AddSetting<T>(this IServiceCollection services)
            where T : SettingDefinition
        {
            return services.AddSingleton<SettingDefinition, T>();
        }

        public static IServiceCollection AddSetting<T>(this IServiceCollection services, Func<IServiceProvider, T> factory)
            where T : SettingDefinition
        {
            return services.AddSingleton<SettingDefinition, T>(factory);
        }

        public static IServiceCollection AddSetting(this IServiceCollection services, Type setting)
        {
            return services.AddSingleton(typeof(SettingDefinition), setting);
        }

        public static IServiceCollection AddSettingSerializer<T>(this IServiceCollection services, Func<T, string> serialize, Func<string, T> deserialize)
        {
            var definition = new RuntimeSettingSerializerDefinition<T>(serialize, deserialize);
            return services.AddSettingSerializer(definition);
        }

        public static IServiceCollection AddSettingSerializer(this IServiceCollection services, SettingSerializerDefinition definition)
        {
            return services.AddSingleton<SettingSerializerDefinition>(definition);
        }

        public static IServiceCollection AddSettingSerializer<T>(this IServiceCollection services)
            where T : SettingSerializerDefinition
        {
            return services.AddSingleton<SettingSerializerDefinition, T>();
        }

        public static IServiceCollection AddSettingSerializer<T>(this IServiceCollection services, Func<IServiceProvider, T> factory)
            where T : SettingSerializerDefinition
        {
            return services.AddSingleton<SettingSerializerDefinition, T>(factory);
        }

        public static IServiceCollection AddSettingSerializer(this IServiceCollection services, Type serializer)
        {
            return services.AddSingleton(typeof(SettingSerializerDefinition), serializer);
        }
    }
}
