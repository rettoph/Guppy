using Guppy.Attributes;
using Guppy.Network.Enums;
using Guppy.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    internal static partial class Settings
    {
        [AutoLoad]
        public class NetworkAuthorizationSerializer : SettingSerializerDefinition<NetworkAuthorization>
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

        [AutoLoad]
        public class HostTypeSerializer : SettingSerializerDefinition<HostType>
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
}
