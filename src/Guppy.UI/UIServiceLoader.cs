using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;

namespace Guppy.UI
{
    [AutoLoad]
    internal sealed class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            // Register Content
            services.AddConfiguration<ContentService>((content, p, c) =>
            {
                content.TryRegister("ui:font", "UI/Font");
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
