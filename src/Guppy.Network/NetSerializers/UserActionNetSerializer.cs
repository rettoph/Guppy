using Guppy.Attributes;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.NetSerializers
{
    [AutoLoad]
    internal sealed class UserActionNetSerializer : NetSerializer<UserAction>
    {
        public override UserAction Deserialize(NetDataReader reader, INetSerializerProvider serializers)
        {
            var instance = new UserAction()
            {
                Id = reader.GetInt(),
                Action = reader.GetEnum<UserAction.Actions>(),
                Claims = new Claim[reader.GetInt()]
            };

            for (var i = 0; i < instance.Claims.Length; i++)
            {
                instance.Claims[i] = Claim.Deserialize(reader);
            }

            return instance;
        }

        public override void Serialize(NetDataWriter writer, INetSerializerProvider serializers, in UserAction instance)
        {
            writer.Put(instance.Id);
            writer.Put(instance.Action);
            writer.Put(instance.Claims.Length);

            foreach (Claim claim in instance.Claims)
            {
                claim.Serialize(writer);
            }
        }
    }
}
