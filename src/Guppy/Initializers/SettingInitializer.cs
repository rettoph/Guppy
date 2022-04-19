using Guppy.Providers;
using Guppy.Initializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.Definitions;
using Guppy.Loaders;
using Minnow.Providers;
using System.Reflection;
using Guppy.Constants;

namespace Guppy.Initializers
{
    [AutoLoad]
    internal sealed class SettingInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var serializers = assemblies.GetAttributes<SettingSerializerDefinition, AutoLoadAttribute>().Types;

            foreach (Type serializer in serializers)
            {
                services.AddSettingSerializer(serializer);
            }

            var settings = assemblies.GetAttributes<SettingDefinition, AutoLoadAttribute>().Types;

            foreach (Type setting in settings)
            {
                services.AddSetting(setting);
            }

            services.AddSingleton<ISettingSerializerProvider, SettingSerializerProvider>();
            services.AddSingleton<ISettingProvider, SettingProvider>();

            services.AddSetting<string>(SettingConstants.CurrentLanguage, SettingConstants.DefaultLanguage, true);
        }
    }
}
