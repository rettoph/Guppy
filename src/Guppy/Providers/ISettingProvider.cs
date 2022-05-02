using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface ISettingProvider : IResourceProvider<ISetting>
    {
        Setting<T> Get<T>();
        Setting<T> Get<T>(string key);
    }
}
