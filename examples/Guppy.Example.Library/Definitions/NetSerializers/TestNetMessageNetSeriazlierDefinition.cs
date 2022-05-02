using Guppy.Attributes;
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
    internal sealed class TestNetMessageNetSeriazlierDefinition : NetSerializerDefinition<TestNetMessage>
    {
        public override void Deserialize(NetDataReader reader, out TestNetMessage instance)
        {
            instance = new TestNetMessage(
                 name: reader.GetString(),
                 age: reader.GetInt(),
                 x: reader.GetInt(),
                 y: reader.GetInt());
        }

        public override void Serialize(NetDataWriter writer, in TestNetMessage instance)
        {
            writer.Put(instance.Name);
            writer.Put(instance.Age);
            writer.Put(instance.X);
            writer.Put(instance.Y);
        }
    }
}
