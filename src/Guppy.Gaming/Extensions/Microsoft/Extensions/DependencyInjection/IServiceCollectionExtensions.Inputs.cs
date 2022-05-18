using Guppy;
using Guppy.Definitions;
using Guppy.Definitions.Settings;
using Guppy.Definitions.SettingSerializers;
using Guppy.Gaming.Definitions;
using Guppy.Gaming.Definitions.Inputs;
using Guppy.Gaming.Structs;
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
        public static IServiceCollection AddInput<TInputDefinition>(this IServiceCollection services)
            where TInputDefinition : InputDefinition
        {
            return services.AddSingleton<InputDefinition, TInputDefinition>();
        }

        public static IServiceCollection AddInput(this IServiceCollection services, Type inputDefinitionType)
        {
            return services.AddSingleton(typeof(InputDefinition), inputDefinitionType);
        }

        public static IServiceCollection AddInput(this IServiceCollection services, InputDefinition inputDefinition)
        {
            return services.AddSingleton<InputDefinition>(inputDefinition);
        }

        public static IServiceCollection AddInput<TOnPress, TOnRelease>(this IServiceCollection services, string key, InputSource defaultSource, Func<IServiceProvider, TOnPress?> onPress, Func<IServiceProvider, TOnRelease?> onRelease)
        {
            return services.AddInput(new RuntimeInputDefinition<TOnPress, TOnRelease>(key, defaultSource, onPress, onRelease));
        }
    }
}
