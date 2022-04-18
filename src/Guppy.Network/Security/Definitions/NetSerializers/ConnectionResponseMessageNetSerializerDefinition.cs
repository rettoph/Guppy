using Guppy.Network.Definitions;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Messages;
using Guppy.Network.Security.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Definitions.NetSerializers
{
    internal sealed class ConnectionResponseMessageNetSerializerDefinition : NetSerializerDefinition<ConnectionResponseMessage>
    {
        public override void Deserialize(NetDataReader reader, out ConnectionResponseMessage instance)
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

        public override void Serialize(NetDataWriter writer, in ConnectionResponseMessage instance)
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
    }
}
