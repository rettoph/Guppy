using Guppy;
using Guppy.Definitions;
using Guppy.Definitions.Settings;
using Guppy.Definitions.SettingSerializers;
using Guppy.Gaming.Definitions;
using Guppy.Providers;
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
        public static IServiceCollection AddCommand<T>(this IServiceCollection services)
            where T : CommandDefinition
        {
            return services.AddSingleton<CommandDefinition, T>();
        }

        public static IServiceCollection AddCommand(this IServiceCollection services, Type commandDefinitionType)
        {
            return services.AddSingleton(typeof(CommandDefinition), commandDefinitionType);
        }
    }
}
