﻿using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Messages;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Serialization.NetSerializers
{
    internal sealed class ConnectionRequestDataNetSerializer : NetSerializer<ConnectionRequestData>
    {
        public override ConnectionRequestData Deserialize(NetDataReader reader)
        {
            ConnectionRequestData instance = new()
            {
                Claims = new Claim[reader.GetInt()]
            };

            for (int i = 0; i < instance.Claims.Length; i++)
            {
                instance.Claims[i] = Claim.Deserialize(reader);
            }

            return instance;
        }

        public override void Serialize(NetDataWriter writer, in ConnectionRequestData instance)
        {
            writer.Put(instance.Claims.Length);

            foreach (Claim claim in instance.Claims)
            {
                claim.Serialize(writer);
            }
        }
    }
}