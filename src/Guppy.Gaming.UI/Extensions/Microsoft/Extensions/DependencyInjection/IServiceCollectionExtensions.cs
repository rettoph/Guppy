using Guppy.Gaming.UI.Definitions;
using Guppy.Gaming.UI.Definitions.FontDefinitions;
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
        public static IServiceCollection AddFont<TFontDefinition>(this IServiceCollection services)
            where TFontDefinition : FontDefinition
        {
            return services.AddSingleton<FontDefinition, TFontDefinition>();
        }

        public static IServiceCollection AddFont(this IServiceCollection services, Type fontDefinitionType)
        {
            return services.AddSingleton(typeof(FontDefinition), fontDefinitionType);
        }

        public static IServiceCollection AddFont(this IServiceCollection services, FontDefinition fontDefinition)
        {
            return services.AddSingleton<FontDefinition>(fontDefinition);
        }

        public static IServiceCollection AddFont(this IServiceCollection services, string key, string trueTypeFontContentKey, int sizePixels)
        {
            return services.AddFont(new RuntimeFontDefinition(key, trueTypeFontContentKey, sizePixels));
        }
    }
}
