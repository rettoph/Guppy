using Guppy.Attributes;
using Guppy.Network.Enums;
using Guppy.Settings.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.SettingSerializers
{
    internal sealed class NetworkAuthorizationSettingSerializer : SettingSerializerDefinition<NetworkAuthorization>
    {
        public override NetworkAuthorization Deserialize(string serialized)
        {
            return Enum.Parse<NetworkAuthorization>(serialized);
        }

        public override string Serialize(NetworkAuthorization deserialized)
        {
            return deserialized.ToString();
        }
    }
}
