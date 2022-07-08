using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.SettingSerializers
{
    internal sealed class StringSettingSerializer : ISettingSerializer<string>
    {
        public Type Type => typeof(string);

        public string Deserialize(string serialized)
        {
            return serialized;
        }

        public string Serialize(string value)
        {
            return value;
        }
    }
}
