using Guppy.Network.Security.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Structs
{
    public readonly struct Claim
    {

        public readonly string Key;
        public readonly string Value;
        public readonly ClaimType Type;
        public readonly DateTime CreatedAt;

        public Claim(string key, string value, ClaimType type)
        {
            this.Key = key;
            this.Value = value;
            this.Type = type;
            this.CreatedAt = DateTime.UtcNow;
        }

        public static Claim Public(string key, string value)
        {
            return new Claim(key, value, ClaimType.Public);
        }

        public static Claim Protected(string key, string value)
        {
            return new Claim(key, value, ClaimType.Protected);
        }

        public static Claim Private(string key, string value)
        {
            return new Claim(key, value, ClaimType.Private);
        }
    }
}
