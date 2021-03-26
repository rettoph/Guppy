using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security
{
    public sealed class Claim
    {
        public readonly String Key;
        public readonly String Value;

        public Claim(String key, String value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
