using Guppy.Attributes;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Settings.Loaders.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Loaders.Definitions.Settings
{
    internal sealed class NetworkAuthorizationSetting : SettingDefinition<NetworkAuthorization>
    {
        public override NetworkAuthorization DefaultValue => NetworkAuthorization.Slave;

        public override string[] Tags => new string[] { SettingConstants.NetworkTag };
    }
}
