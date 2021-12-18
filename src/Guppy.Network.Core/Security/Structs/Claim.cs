using Guppy.Network.Security.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Structs
{
    public readonly struct Claim
    {
        
        public readonly String Key;
        public readonly String Value;
        public readonly ClaimType Type;

        public Claim(string name, string value, ClaimType type)
        {
            this.Key = name;
            this.Value = value;
            this.Type = type;
        }
    }
}
