using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface ISetting
    {
        string Key { get; }
        bool Exportable { get; }
        string[] Tags { get; }
        Type Type { get; }
        ISettingSerializer Serializer { get; }
    }

    public interface ISetting<T> : ISetting
    {
        T Value { get; set; }
        T DefaultValue { get; }
        new ISettingSerializer<T> Serializer { get; }
    }
}
