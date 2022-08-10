using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Structs
{
    public readonly struct NetId
    {
        public readonly byte Value;

        public static int SizeInBytes { get; } = 1;

        public NetId(byte value)
        {
            this.Value = value;
        }

        public static NetId operator +(NetId addend1, byte addend2)
        {
            byte sum = (byte)(addend1.Value + addend2);
            return new NetId(sum);
        }

        public static implicit operator NetId(byte value)
        {
            return new NetId(value);
        }


        public void Write(NetDataWriter writer)
        {
            writer.Put(this.Value);
        }

        public static NetId Read(NetDataReader reader)
        {
            return new NetId(reader.GetByte());
        }
    }
}
