using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.SettingSerializers
{
    internal sealed class Int32SettingSerializer : ISettingSerializer<int>
    {
        public Type Type => typeof(int);

        public int Deserialize(string serialized)
        {
            return int.Parse(serialized);
        }

        public string Serialize(int value)
        {
            return value.ToString();
        }
    }
}
