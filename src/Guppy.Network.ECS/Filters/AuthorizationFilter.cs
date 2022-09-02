using Guppy.Common;
using Guppy.Network.Enums;
using Guppy.Resources;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ECS.Filters
{
    public abstract class AuthorizationFilter : IFilter
    {
        private readonly ISetting<NetAuthorization> _currentAuthorization;

        public abstract NetAuthorization RequiredAuthorization { get; }

        internal AuthorizationFilter(ISettingProvider settings)
        {
            _currentAuthorization = settings.Get<NetAuthorization>();
        }

        public bool Invoke()
        {
            return _currentAuthorization.Value == this.RequiredAuthorization; 
        }
    }
}
