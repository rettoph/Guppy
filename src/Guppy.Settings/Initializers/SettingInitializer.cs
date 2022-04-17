using Guppy.Settings.Initializers.Collections;
using Guppy.Settings.Loaders;
using Guppy.Settings.Providers;
using Guppy.Initializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Attributes;
using Guppy.Settings.Loaders.Definitions;

namespace Guppy.Settings.Initializers
{
    internal sealed class SettingInitializer : GuppyInitializer<ISettingLoader>
    {
        protected override void Initialize(AssemblyHelper assemblies, IServiceCollection services, IEnumerable<ISettingLoader> loaders)
        {
            var serializerDescriptors = assemblies.Types.GetTypesWithAttribute<SettingSerializerDefinition, AutoLoadAttribute>()
                .Select(x => Activator.CreateInstance(x) as SettingSerializerDefinition)
                .Select(x => x.BuildDescriptor());

            var settingDescriptors = assemblies.Types.GetTypesWithAttribute<SettingDefinition, AutoLoadAttribute>()
                .Select(x => Activator.CreateInstance(x) as SettingDefinition)
                .Select(x => x.BuildDescriptor());

            var serializers = new SettingSerializerCollection(serializerDescriptors);
            var settings = new SettingCollection(settingDescriptors);

            

            foreach (ISettingLoader loader in loaders)
            {
                loader.ConfigureSettings(settings, serializers);
            }

            var provider = settings.BuildSettingProvider(serializers);
            services.AddSingleton<ISettingProvider>(provider);
        }
    }
}
