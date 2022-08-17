using Guppy;
using Guppy.Providers;
using Guppy.Resources;
using Guppy.Resources.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSetting<T>(this IServiceCollection services)
            where T : class, ISettingDefinition
        {
            return services.AddSingleton<ISettingDefinition, T>();
        }

        public static IServiceCollection AddSetting(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(ISettingDefinition), definitionType);
        }

        public static IServiceCollection AddSetting<T>(this IServiceCollection services, string key, T defaultValue, bool exportable, params string[] tags)
            where T : notnull
        {
            return services.AddSingleton<ISettingDefinition>(new RuntimeSettingDefinition<T>(key, defaultValue, exportable, tags));
        }

        public static IServiceCollection AddSetting<T>(this IServiceCollection services, T defaultValue, bool exportable, params string[] tags)
            where T : notnull
        {
            return services.AddSingleton<ISettingDefinition>(new RuntimeSettingDefinition<T>(typeof(T).FullName ?? throw new Exception(), defaultValue, exportable, tags));
        }
    }
}
