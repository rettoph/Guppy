using Guppy.Attributes;
using Guppy.Network.Constants;
using Guppy.Network.Enums;
using Guppy.Settings.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.Settings
{
    internal sealed class HostTypeSetting : SettingDefinition<HostType>
    {
        public override HostType DefaultValue => HostType.Local;

        public override string[] Tags => new string[] { SettingConstants.NetworkTag };
    }
}
