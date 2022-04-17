using Guppy.Settings.Providers;
using Guppy.Initializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.Settings.Definitions;
using Guppy.Loaders;

namespace Guppy.Settings.Initializers
{
    internal sealed class SettingInitializer : IGuppyInitializer
    {
        public void Initialize(AssemblyHelper assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var serializers = assemblies.Types.GetTypesWithAttribute<SettingSerializerDefinition, AutoLoadAttribute>();

            foreach (Type serializer in serializers)
            {
                services.AddSettingSerializer(serializer);
            }

            var settings = assemblies.Types.GetTypesWithAttribute<SettingDefinition, AutoLoadAttribute>();

            foreach (Type setting in settings)
            {
                services.AddSetting(setting);
            }

            services.AddSingleton<ISettingSerializerProvider, SettingSerializerProvider>();
            services.AddSingleton<ISettingProvider, SettingProvider>();
        }
    }
}
