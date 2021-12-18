using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteNetLib.Utils
{
    internal static class NetDataReaderSecurityExtensions
    {
        public static IEnumerable<Claim> GetClaims(this NetDataReader reader)
        {
            List<Claim> claimsList = new List<Claim>();
            while (reader.GetBool())
            {
                claimsList.Add(new Claim(
                    reader.GetString(),
                    reader.GetString(),
                    reader.GetEnum<ClaimType>()));
            }

            return claimsList;
        }
    }
}
