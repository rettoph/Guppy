using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Attributes
{
    public sealed class NetworkAuthorizationRequiredAttribute : Attribute
    {
        public readonly NetworkAuthorization NetworkAuthorization;

        public NetworkAuthorizationRequiredAttribute(NetworkAuthorization networkAuthorization)
        {
            this.NetworkAuthorization = networkAuthorization;
        }
    }
}
