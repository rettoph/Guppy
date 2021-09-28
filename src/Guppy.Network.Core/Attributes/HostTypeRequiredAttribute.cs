using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Attributes
{
    public sealed class HostTypeRequiredAttribute : Attribute
    {
        public readonly HostType HostType;

        public HostTypeRequiredAttribute(HostType hostType)
        {
            this.HostType = hostType;
        }
    }
}
