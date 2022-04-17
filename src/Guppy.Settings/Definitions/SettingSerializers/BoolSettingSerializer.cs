using Guppy.Attributes;
using Guppy.Settings.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Definitions.SettingSerializers
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
