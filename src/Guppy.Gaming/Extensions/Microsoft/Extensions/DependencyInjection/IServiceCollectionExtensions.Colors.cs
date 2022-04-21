using Guppy;
using Guppy.Definitions;
using Guppy.Definitions.Settings;
using Guppy.Definitions.SettingSerializers;
using Guppy.Gaming.Definitions;
using Guppy.Gaming.Definitions.ColorDefinitions;
using Guppy.Gaming.Definitions.InputDefinitions;
using Guppy.Gaming.Structs;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddColor<TColorDefinition>(this IServiceCollection services)
            where TColorDefinition : ColorDefinition
        {
            return services.AddSingleton<ColorDefinition, TColorDefinition>();
        }

        public static IServiceCollection AddColor(this IServiceCollection services, Type colorDefinitionType)
        {
            return services.AddSingleton(typeof(ColorDefinition), colorDefinitionType);
        }

        public static IServiceCollection AddColor(this IServiceCollection services, ColorDefinition colorDefinition)
        {
            return services.AddSingleton<ColorDefinition>(colorDefinition);
        }

        public static IServiceCollection AddColor(this IServiceCollection services, string key, XnaColor value)
        {
            return services.AddColor(new RuntimeColorDefinition(key, value));
        }
    }
}
