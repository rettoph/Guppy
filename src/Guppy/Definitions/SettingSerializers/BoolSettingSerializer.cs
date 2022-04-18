using Guppy.Attributes;
using Guppy.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Definitions.SettingSerializers
{
    [AutoLoad]
    internal sealed class BoolSettingSerializer : SettingSerializerDefinition<bool>
    {
        public override bool Deserialize(string serialized)
        {
            return bool.Parse(serialized);
        }

        public override string Serialize(bool deserialized)
        {
            return deserialized.ToString();
        }
    }
}
