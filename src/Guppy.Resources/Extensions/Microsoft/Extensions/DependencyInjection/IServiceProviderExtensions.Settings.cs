using Guppy.Resources;
using Guppy.Resources.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceProviderExtensions
    {
        public static ISetting<T> GetSetting<T>(this IServiceProvider provider)
        {
            return provider.GetRequiredService<ISettingProvider>().Get<T>();
        }

        public static ISetting<T> GetSetting<T>(this IServiceProvider provider, string key)
        {
            return provider.GetRequiredService<ISettingProvider>().Get<T>(key);
        }
    }
}
