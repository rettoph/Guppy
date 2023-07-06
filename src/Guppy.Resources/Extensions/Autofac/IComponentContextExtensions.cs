using Guppy.Resources;
using Guppy.Resources.Providers;

namespace Autofac
{
    public static class IServiceProviderExtensions
    {
        public static ISetting<T> GetSetting<T>(this IComponentContext context)
        {
            return context.Resolve<ISettingProvider>().Get<T>();
        }

        public static ISetting<T> GetSetting<T>(this IComponentContext context, string key)
        {
            return context.Resolve<ISettingProvider>().Get<T>(key);
        }
    }
}
