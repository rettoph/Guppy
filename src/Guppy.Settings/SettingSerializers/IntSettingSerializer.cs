using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.SettingSerializers
{
    [AutoLoad]
    internal sealed class IntSettingSerializer : SettingSerializerDefinition<int>
    {
        public override int Deserialize(string serialized)
        {
            return int.Parse(serialized);
        }

        public override string Serialize(int deserialized)
        {
            return deserialized.ToString();
        }
    }
}
