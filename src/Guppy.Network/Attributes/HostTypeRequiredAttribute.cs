using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
