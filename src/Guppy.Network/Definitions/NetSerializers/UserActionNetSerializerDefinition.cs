using Guppy.Attributes;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Messages;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetSerializers
{
    [AutoLoad]
    internal sealed class UserActionNetSerializerDefinition : NetSerializerDefinition<UserAction>
    {
        public override void Deserialize(NetDataReader reader, out UserAction instance)
        {
            instance = new UserAction()
            {
                Id = reader.GetInt(),
                Action = reader.GetEnum<UserAction.Actions>(),
                Claims = new Claim[reader.GetInt()]
            };

            for(var i=0; i<instance.Claims.Length; i++)
            {
                instance.Claims[i] = Claim.Deserialize(reader);
            }
        }

        public override void Serialize(NetDataWriter writer, in UserAction instance)
        {
            writer.Put(instance.Id);
            writer.Put(instance.Action);
            writer.Put(instance.Claims.Length);

            foreach(Claim claim in instance.Claims)
            {
                claim.Serialize(writer);
            }
        }
    }
}
