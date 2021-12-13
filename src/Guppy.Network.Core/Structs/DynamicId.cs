using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Structs
{
    public struct DynamicId
    {
        public readonly UInt16 Value;
        public readonly Byte[] Bytes;

        public DynamicId(
            UInt16 value, 
            Byte[] bytes)
        {
            this.Value = value;
            this.Bytes = bytes;
        }

        public override bool Equals(object obj)
        {
            return obj is DynamicId id && this.Value == id.Value;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        #region Operator Overloads
        public static bool operator ==(DynamicId id1, UInt16 id2)
        {
            return id1.Value == id2;
        }

        public static bool operator !=(DynamicId id1, UInt16 id2)
        {
            return id1.Value != id2;
        }

        public static bool operator ==(DynamicId id1, DynamicId id2)
        {
            return id1.Value == id2.Value;
        }

        public static bool operator !=(DynamicId id1, DynamicId id2)
        {
            return id1.Value != id2.Value;
        }
        #endregion

        #region Static Methods
        #endregion
    }
}
