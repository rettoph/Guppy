using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Loaders
{
    [Service<ISettingLoader>(ServiceLifetime.Singleton, true)]
    public interface ISettingLoader
    {
        void Load(ISettingProvider settings);
    }
}
