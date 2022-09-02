using Guppy.Attributes;
using Guppy.Example.Library.Messages;
using Guppy.Network.Definitions;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Definitions.NetSerializers
{
    [AutoLoad]
    internal sealed class CreateEntityNetSerializerDefinition : NetSerializerDefinition<CreateEntity>
    {
        public override void Deserialize(NetDataReader reader, out CreateEntity instance)
        {
            instance = new CreateEntity(reader.GetUInt());
        }

        public override void Serialize(NetDataWriter writer, in CreateEntity instance)
        {
            writer.Put(instance.Id);
        }
    }
}
