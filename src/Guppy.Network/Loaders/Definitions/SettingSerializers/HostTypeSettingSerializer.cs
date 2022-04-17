using Guppy.Attributes;
using Guppy.Network.Enums;
using Guppy.Settings.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders.Definitions.SettingSerializers
{
    internal sealed class HostTypeSettingSerializer : SettingSerializerDefinition<HostType>
    {
        public override HostType Deserialize(string serialized)
        {
            return Enum.Parse<HostType>(serialized);
        }

        public override string Serialize(HostType deserialized)
        {
            return deserialized.ToString();
        }
    }
}
