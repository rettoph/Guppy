using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library
{
    public struct TestNetMessage
    {
        public string Name;
        public int Age;
        public int X;
        public int Y;

        public TestNetMessage(string name, int age, int x, int y)
        {
            this.Name = name;
            this.Age = age;
            this.X = x;
            this.Y = y;
        }

        internal static void Serialize(NetDataWriter writer, in TestNetMessage instance)
        {
            writer.Put(instance.Name);
            writer.Put(instance.Age);
            writer.Put(instance.X);
            writer.Put(instance.Y);
        }

        internal static void Deserialize(NetDataReader reader, out TestNetMessage instance)
        {
            instance = new TestNetMessage(
                name: reader.GetString(),
                age: reader.GetInt(),
                x: reader.GetInt(),
                y: reader.GetInt());
        }
    }
}
