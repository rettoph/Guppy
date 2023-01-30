using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Resources.Constants;
using Guppy.Resources.Definitions;
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

namespace Guppy.Resources.Initializers
{
    internal sealed class SettingInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var definitions = assemblies.GetTypes<ISettingDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach(Type definition in definitions)
            {
                services.AddSetting(definition);
            }

            services.AddSingleton<ISettingProvider, SettingProvider>()
                    .AddSingleton<IJsonSerializer, JsonSerializer>()
                    .AddSingleton<JsonConverter>(new JsonStringEnumConverter(STJ.JsonNamingPolicy.CamelCase));

            services.AddSetting(SettingConstants.CurrentLanguage, LanguageConstants.Default, true, nameof(Guppy));
        }
    }
}
