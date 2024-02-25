using Guppy.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Messages;
using LiteNetLib.Utils;

namespace Guppy.Network.Serialization.NetSerializers
{
    [AutoLoad]
    internal sealed class UserActionNetSerializer : NetSerializer<UserAction>
    {
        public override UserAction Deserialize(NetDataReader reader)
        {
            var instance = new UserAction()
            {
                Id = reader.GetInt(),
                Type = reader.GetEnum<UserActionTypes>(),
                Claims = new Claim[reader.GetInt()]
            };

            for (var i = 0; i < instance.Claims.Length; i++)
            {
                instance.Claims[i] = Claim.Deserialize(reader);
            }

            return instance;
        }

        public override void Serialize(NetDataWriter writer, in UserAction instance)
        {
            writer.Put(instance.Id);
            writer.Put(instance.Type);
            writer.Put(instance.Claims.Length);

            foreach (Claim claim in instance.Claims)
            {
                claim.Serialize(writer);
            }
        }
    }
}
