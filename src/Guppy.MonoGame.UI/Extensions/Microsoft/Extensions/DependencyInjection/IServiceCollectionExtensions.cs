using Guppy.MonoGame.UI.Definitions;
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
        public static IServiceCollection AddImGuiFont<TImGuiFontDefinition>(this IServiceCollection services)
            where TImGuiFontDefinition : class, IImGuiFontDefinition
        {
            return services.AddSingleton<IImGuiFontDefinition, TImGuiFontDefinition>();
        }

        public static IServiceCollection AddImGuiFont(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(IImGuiFontDefinition), definitionType);
        }

        public static IServiceCollection AddImGuiFont(this IServiceCollection services, IImGuiFontDefinition fontDefinition)
        {
            return services.AddSingleton<IImGuiFontDefinition>(fontDefinition);
        }

        public static IServiceCollection AddImGuiFont(this IServiceCollection services, string key, string trueTypeFontResourceName, int sizePixels)
        {
            return services.AddImGuiFont(new RuntimeImGuiFontDefinition(key, trueTypeFontResourceName, sizePixels));
        }
    }
}
