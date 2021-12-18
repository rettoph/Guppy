using Guppy.Network.Security.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteNetLib.Utils
{
    internal static class NetDataWriterSecurityExtensions
    {
        public static void Put(this NetDataWriter writer, IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                writer.Put(true);
                writer.Put(claim.Key);
                writer.Put(claim.Value);
                writer.Put(claim.Type);
            }

            writer.Put(false);
        }
    }
}
