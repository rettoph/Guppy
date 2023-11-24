using Autofac;
using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.Loaders;
using Guppy.Resources.Constants;
using Guppy.Resources.Providers;
using Guppy.Resources.Serialization.Json.Converters;
using Guppy.Serialization;
using System.Reflection;
using System.Text.Json.Serialization;
using STJ = System.Text.Json;

namespace Guppy.Resources.Loaders
{
    [AutoLoad]
    internal sealed class SettingLoader : ISettingLoader
    {
        public IEnumerable<Setting> GetSettings(ISettingProvider settings)
        {
            yield return Settings.Localization;
        }

        public void Load(ISettingProvider settings)
        {
            settings.Register(Settings.Localization, Localization.en_US);
        }
    }
}
