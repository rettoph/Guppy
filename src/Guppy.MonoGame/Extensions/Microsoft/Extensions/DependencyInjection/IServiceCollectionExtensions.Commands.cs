using Guppy;
using Guppy.MonoGame.Definitions;
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
            where T : class, ICommandDefinition
        {
            return services.AddSingleton<ICommandDefinition, T>();
        }

        public static IServiceCollection AddCommand(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(ICommandDefinition), definitionType);
        }
    }
}
