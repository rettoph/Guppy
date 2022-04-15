using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.Network.Constants;
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
        public class NetworkAuthorizationSetting : SettingDefinition<NetworkAuthorization>
        {
            public override NetworkAuthorization DefaultValue => NetworkAuthorization.Slave;

            public override bool Exportable => false;

            public override string[] Tags => new string[] { SettingConstants.NetworkTag };
        }

        [AutoLoad]
        public class HostTypeSetting : SettingDefinition<HostType>
        {
            public override HostType DefaultValue => HostType.Local;

            public override bool Exportable => false;

            public override string[] Tags => new string[] { SettingConstants.NetworkTag };
        }
    }
}
