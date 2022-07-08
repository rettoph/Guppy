using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface ISettingSerializer
    {
        Type Type { get; }
    }

    public interface ISettingSerializer<T> : ISettingSerializer
    {
        string Serialize(T value);
        T Deserialize(string serialized);
    }
}
