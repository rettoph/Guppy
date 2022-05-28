using Guppy.Network.Enums;
using Guppy.Network.Messages;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Definitions.NetSerializers
{
    internal sealed class NetTargetActionMessageNetSerializerDefinition : NetSerializerDefinition<NetTargetActionMessage>
    {
        public override void Deserialize(NetDataReader reader, out NetTargetActionMessage instance)
        {
            instance = new NetTargetActionMessage(
                id: reader.GetUShort(),
                typeHash: reader.GetUInt(),
                action: reader.GetEnum<NetTargetAction>());
        }

        public override void Serialize(NetDataWriter writer, in NetTargetActionMessage instance)
        {
            writer.Put(instance.Id);
            writer.Put(instance.TypeHash);
            writer.Put(instance.Action);
        }
    }
}
