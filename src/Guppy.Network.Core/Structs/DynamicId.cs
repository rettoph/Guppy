using Guppy.Network.Enums;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Structs
{
    public readonly struct DynamicId
    {
        public readonly UInt16 Value;
        public readonly Byte[] Bytes;

        public DynamicId(
            UInt16 value,
            DynamicIdSize size)
        {
            this.Value = value;

            // TODO: Research big endian vs little endian issues with this?
            if (size == DynamicIdSize.OneByte)
            {
                this.Bytes = new[] { (Byte)value };
            }
            else
            {
                this.Bytes = BitConverter.GetBytes(value);
            }
        }

        public DynamicId(
            UInt16 value, 
            Byte[] bytes)
        {
            this.Value = value;
            this.Bytes = bytes;
        }

        public DynamicIdSize GetSize()
        {
            if(this.Bytes.Length == 1)
            {
                return DynamicIdSize.OneByte;
            }

            if(this.Bytes.Length == 2)
            {
                return DynamicIdSize.TwoBytes;
            }

            throw new ArgumentException();
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

        public static DynamicId operator ++(in DynamicId a)
            => new DynamicId((UInt16)(a.Value + 1), a.GetSize());
        #endregion

        #region Static Methods
        public static DynamicIdSize GetSize(Int32 maxValue)
        {
            if (maxValue > UInt16.MaxValue)
            {
                throw new ArgumentException(nameof(maxValue));
            }

            return DynamicId.GetSize((UInt16)maxValue);
        }

        public static DynamicIdSize GetSize(UInt16 maxValue)
        {
            if(maxValue <= Byte.MaxValue)
            {
                return DynamicIdSize.OneByte;
            }

            return DynamicIdSize.TwoBytes;
        }

        public static Action<NetDataWriter, Byte[]> GetNetWriter(DynamicIdSize size)
        {
            void DynamicIdWriter(NetDataWriter writer, Byte[] bytes)
            {
                writer.Put(bytes);
            }

            return DynamicIdWriter;
        }

        public static Func<NetDataReader, UInt16> GetNetReader(DynamicIdSize size)
        {
            if(size == DynamicIdSize.OneByte)
            {
                UInt16 OneByteReader(NetDataReader reader)
                {
                    return reader.GetByte();
                }

                return OneByteReader;
            }

            Byte[] buffer = new Byte[2];
            UInt16 TwoByteReader(NetDataReader reader)
            {
                reader.GetBytes(buffer, 2);
                return BitConverter.ToUInt16(buffer);
            }

            return TwoByteReader;
        }
        #endregion
    }
}
