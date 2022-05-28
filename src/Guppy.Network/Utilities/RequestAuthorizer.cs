using Guppy.Network.Enums;
using Guppy.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    public sealed class RequestAuthorizer
    {
        public readonly Setting<NetworkAuthorization> CurrentNetworkAuthorization;
        public readonly Setting<HostType> CurrentHostType;
        public readonly NetScope CurrentScope;

        public RequestAuthorizer(NetScope scope, ISettingProvider settings)
        {
            this.CurrentScope = scope;
            this.CurrentNetworkAuthorization = settings.Get<NetworkAuthorization>();
            this.CurrentHostType = settings.Get<HostType>();
        }

        public bool Unauthorized(NetIncomingMessage request)
        {
            request.Sender?.Disconnect();
            return false;
        }


        public bool Authorize(NetworkAuthorization authorization, NetIncomingMessage request)
        {
            if(this.CurrentNetworkAuthorization.Value == authorization)
            {
                return true;
            }

            return this.Unauthorized(request);
        }
    }
}
