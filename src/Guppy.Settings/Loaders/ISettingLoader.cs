using Guppy.Settings.Loaders.Collections;
using Guppy.Settings.Providers;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Loaders
{
    public interface ISettingLoader : IGuppyLoader
    {
        void ConfigureSettingSerializers(ISettingSerializerCollection serializers);
        void ConfigureSettings(ISettingCollection settings);
        void ImportSettings(ISettingProvider settings);
    }
}
