using Guppy.Gaming.Definitions;
using Guppy.Gaming.Definitions.Colors;
using Guppy.Gaming.Definitions.Content;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddContent<TContentDefinition>(this IServiceCollection services)
            where TContentDefinition : ContentDefinition
        {
            return services.AddSingleton<ContentDefinition, TContentDefinition>();
        }

        public static IServiceCollection AddContent(this IServiceCollection services, Type contentDefinitionType)
        {
            return services.AddSingleton(typeof(ContentDefinition), contentDefinitionType);
        }

        public static IServiceCollection AddContent(this IServiceCollection services, ContentDefinition contentDefinition)
        {
            return services.AddSingleton<ContentDefinition>(contentDefinition);
        }

        public static IServiceCollection AddContent<T>(this IServiceCollection services, string key, string path)
        {
            return services.AddContent(new RuntimeContentDefinition<T>(key, path));
        }
    }
}
