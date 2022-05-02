using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetSerializers
{
    internal sealed class UserActionMessageNetSerializerDefinition : NetSerializerDefinition<UserActionMessage>
    {
        public override void Deserialize(NetDataReader reader, out UserActionMessage instance)
        {
            int id = reader.GetInt();
            UserActionType action = reader.GetEnum<UserActionType>();

            List<Claim> claims = new List<Claim>();
            while (reader.GetBool())
            {
                claims.Add(new Claim(
                    key: reader.GetString(),
                    value: reader.GetString(),
                    type: reader.GetEnum<ClaimType>()));
            }

            instance = new UserActionMessage(id, claims, action);
        }

        public override void Serialize(NetDataWriter writer, in UserActionMessage instance)
        {
            writer.Put(instance.Id);
            writer.Put(instance.Type);

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
