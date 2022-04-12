using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Structs
{
    public readonly struct DynamicId
    {
        public readonly ushort Value;
        public readonly byte[] Bytes;

        internal DynamicId(ushort value, byte[] bytes)
        {
            this.Value = value;
            this.Bytes = bytes;
        }

        public override bool Equals(object? obj)
        {
            return obj is DynamicId id && this.Value == id.Value;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(in DynamicId id1, in UInt16 id2)
        {
            return id1.Value == id2;
        }

        public static bool operator !=(in DynamicId id1, in UInt16 id2)
        {
            return id1.Value != id2;
        }

        public static bool operator ==(in DynamicId id1, in DynamicId id2)
        {
            return id1.Value == id2.Value;
        }

        public static bool operator !=(in DynamicId id1, in DynamicId id2)
        {
            return id1.Value != id2.Value;
        }
    }
}
