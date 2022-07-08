using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface ISettingProvider
    {
        ISetting<T> Get<T>();
        ISetting<T> Get<T>(string key);
    }
}
