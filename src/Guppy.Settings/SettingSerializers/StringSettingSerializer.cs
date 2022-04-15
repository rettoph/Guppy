using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.SettingSerializers
{
    [AutoLoad]
    internal sealed class StringSettingSerializer : SettingSerializerDefinition<string>
    {
        public override string Deserialize(string serialized)
        {
            return serialized;
        }

        public override string Serialize(string deserialized)
        {
            return deserialized;
        }
    }
}
