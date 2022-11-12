using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Resources.Definitions;
using Guppy.Resources.Providers;
using Guppy.Resources.SettingSerializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    .AddSingleton<ISettingTypeSerializer, StringSettingSerializer>()
                    .AddSingleton<ISettingTypeSerializer, Int32SettingSerializer>()
                    .AddSingleton<ISettingTypeSerializer, TimeSpanSerializer>();
        }
    }
}
