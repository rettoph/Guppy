using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public static class NetId
    {
        public static INetId Create<T>(T value)
        {
            if(typeof(T) == typeof(byte))
            {
                return new NetId.Byte()
                {
                    Value = Convert.ToByte(value)
                };
            }

            if (typeof(T) == typeof(ushort))
            {
                return new NetId.UShort()
                {
                    Value = Convert.ToUInt16(value)
                };
            }

            throw new ArgumentException();
        }

        public readonly struct Byte : INetId<byte>
        {
            public static int SizeInBytes { get; } = 1;

            public byte Value { get; init; }

            public void Write(NetDataWriter writer)
            {
                writer.Put(this.Value);
            }

            public static INetId Read(NetDataReader reader)
            {
                return NetId.Byte.Create(reader.GetByte());
            }

            public static INetId<byte> Create(byte value)
            {
                return new NetId.Byte()
                {
                    Value = value
                };
            }

            public bool Equals(INetId? other)
            {
                if(other is NetId.Byte casted)
                {
                    return casted.Value == this.Value;
                }

                return false;
            }

            public INetId<byte> Next()
            {
                return new NetId.Byte()
                {
                    Value = (byte)(this.Value + 1)
                };
            }

            INetId INetId.Next()
            {
                return this.Next();
            }
        }

        public readonly struct UShort : INetId<ushort>
        {
            public static int SizeInBytes { get; } = 2;

            public ushort Value { get; init; }

            public void Write(NetDataWriter writer)
            {
                writer.Put(this.Value);
            }

            public static INetId Read(NetDataReader reader)
            {
                return NetId.UShort.Create(reader.GetUShort());
            }

            public static INetId<ushort> Create(ushort value)
            {
                return new NetId.UShort()
                {
                    Value = value
                };
            }

            public bool Equals(INetId? other)
            {
                if (other is NetId.UShort casted)
                {
                    return casted.Value == this.Value;
                }

                return false;
            }

            public INetId<ushort> Next()
            {
                return new NetId.UShort()
                {
                    Value = (ushort)(this.Value + 1)
                };
            }

            INetId INetId.Next()
            {
                return this.Next();
            }
        }
    }
}
