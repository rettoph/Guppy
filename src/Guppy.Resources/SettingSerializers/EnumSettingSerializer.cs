using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.SettingSerializers
{
    public class EnumSettingSerializer<T> : ISettingSerializer<T>
        where T : struct, Enum
    {
        public Type Type => typeof(T);

        public T Deserialize(string serialized)
        {
            return Enum.Parse<T>(serialized);
        }

        public string Serialize(T value)
        {
            return value.ToString();
        }
    }
}
