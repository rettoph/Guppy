using Guppy.Network.Security.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security
{
    public struct Claim
    {
        public readonly ClaimType Type;
        public readonly String Value;

        public Claim(ClaimType type, String value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
