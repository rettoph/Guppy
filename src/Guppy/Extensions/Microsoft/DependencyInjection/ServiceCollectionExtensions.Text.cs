using Guppy.Definitions;
using Guppy.Definitions.Texts;
using Guppy.Providers;
using Minnow.System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddText(this IServiceCollection services, Type textDefinitionType)
        {
            return services.AddSingleton(typeof(TextDefinition), textDefinitionType);
        }

        public static IServiceCollection AddText<TDefinition>(this IServiceCollection services)
            where TDefinition : TextDefinition
        {
            return services.AddSingleton<TextDefinition, TDefinition>();
        }

        public static IServiceCollection AddText(this IServiceCollection services, TextDefinition definition)
        {
            return services.AddSingleton<TextDefinition>(definition);
        }

        public static IServiceCollection AddText(this IServiceCollection services, string key, string? defaultValue)
        {
            return services.AddText(new RuntimeTextDefinition(key, defaultValue));
        }
    }
}
