using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface ISettingProvider : IEnumerable<Setting>
    {
        Setting this[string key] { get; }

        Setting<T> Get<T>();
        Setting<T> Get<T>(string key);
    }
}
