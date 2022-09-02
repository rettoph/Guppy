using Guppy.Network.Enums;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ECS.Filters
{
    public sealed class SlaveAuthorizationFilter : AuthorizationFilter
    {
        public SlaveAuthorizationFilter(ISettingProvider settings) : base(settings)
        {
        }

        public override NetAuthorization RequiredAuthorization => NetAuthorization.Slave;
    }
}
