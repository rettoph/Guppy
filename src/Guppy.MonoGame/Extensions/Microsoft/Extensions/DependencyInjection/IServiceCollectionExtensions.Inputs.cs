using Guppy;
using Guppy.MonoGame;
using Guppy.MonoGame.Definitions;
using Guppy.MonoGame.Definitions.Inputs;
using Guppy.MonoGame.Structs;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInput<T>(this IServiceCollection services)
            where T : class, IInputDefinition
        {
            return services.AddSingleton<IInputDefinition, T>();
        }

        public static IServiceCollection AddInput(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(IInputDefinition), definitionType);
        }

        public static IServiceCollection AddInput<TData>(this IServiceCollection services, string key, InputSource defaultSource, (ButtonState, TData)[] data)
            where TData : ICommandData
        {
            return services.AddSingleton(typeof(IInputDefinition), new RuntimeInputDefinition<TData>(key, defaultSource, data));
        }
    }
}
