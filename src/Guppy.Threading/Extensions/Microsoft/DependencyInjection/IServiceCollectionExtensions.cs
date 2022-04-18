using Guppy.Threading.Definitions;
using Guppy.Threading.Definitions.BusMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBusMessage(this IServiceCollection services, Type busMessageDefinitionType)
        {
            return services.AddSingleton(typeof(BusMessageDefinition), busMessageDefinitionType);
        }
        public static IServiceCollection AddBusMessage<TDefinition>(this IServiceCollection services)
            where TDefinition : BusMessageDefinition
        {
            return services.AddSingleton<BusMessageDefinition, TDefinition>();
        }
        public static IServiceCollection AddBusMessage(this IServiceCollection services, BusMessageDefinition definition)
        {
            return services.AddSingleton<BusMessageDefinition>(definition);
        }
        public static IServiceCollection AddBusMessage<T>(this IServiceCollection services, int queue)
        {
            return services.AddBusMessage(new RuntimeBusMessageDefinition<T>(queue));
        }

    }
}
