using Guppy.Common;
using Guppy.Loaders;
using Guppy.Resources.Constants;
using Guppy.Resources.Providers;
using Guppy.Resources.Serialization.Json;
using Guppy.Resources.Serialization.Json.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using STJ = System.Text.Json;

namespace Guppy.Resources.Loaders
{
    internal sealed class SettingLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISettingProvider, SettingProvider>()
                    .AddSingleton<IJsonSerializer, JsonSerializer>()
                    .AddSingleton<JsonConverter>(new JsonStringEnumConverter(STJ.JsonNamingPolicy.CamelCase));

            services.AddSetting(SettingConstants.CurrentLanguage, LanguageConstants.Default, true, nameof(Guppy));
        }
    }
}
