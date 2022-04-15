using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Messages
{
    public struct ConnectionResponseMessage
    {
        public readonly int Id;
        public readonly Claim[] Claims;

        public ConnectionResponseMessage(int id, Claim[] claims)
        {
            this.Id = id;
            this.Claims = claims;
        }

        internal static void Serialize(NetDataWriter writer, in ConnectionResponseMessage instance)
        {
            writer.Put(instance.Id);

            foreach (Claim claim in instance.Claims)
            {
                if (claim.Type != ClaimType.Private)
                {
                    writer.Put(true);
                    writer.Put(claim.Key);
                    writer.Put(claim.Value);
                    writer.Put(claim.Type);
                }
            }

            writer.Put(false);
        }

        internal static void Deserialize(NetDataReader reader, out ConnectionResponseMessage instance)
        {
            int id = reader.GetInt();

            List<Claim> claims = new List<Claim>();
            while (reader.GetBool())
            {
                claims.Add(new Claim(
                    key: reader.GetString(),
                    value: reader.GetString(),
                    type: reader.GetEnum<ClaimType>()));
            }

            instance = new ConnectionResponseMessage(id, claims.ToArray());
        }
    }
}
