using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface ISettingTypeSerializer
    {
        Type Type { get; }
    }

    public interface ISettingSerializer<T> : ISettingTypeSerializer
    {
        string Serialize(T value);
        T Deserialize(string serialized);
    }
}
