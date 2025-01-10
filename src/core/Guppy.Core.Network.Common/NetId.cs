using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common
{
    public static class NetId
    {
        public static INetId<T> Create<T>(T value)
        {
            if (typeof(T) == typeof(byte))
            {
                return new NetId.Byte()
                {
                    Value = Convert.ToByte(value)
                } as INetId<T> ?? throw new Exception();
            }

            if (typeof(T) == typeof(ushort))
            {
                return new NetId.UShort()
                {
                    Value = Convert.ToUInt16(value)
                } as INetId<T> ?? throw new Exception();
            }

            throw new ArgumentException(nameof(value));
        }

        public readonly struct Byte : INetId<byte>
        {
            public static INetId Zero { get; } = Create(0);

            public static byte SizeInBytes { get; } = 1;

            public byte Value { get; init; }

            int INetId.Value => this.Value;

            public void Write(NetDataWriter writer)
            {
                writer.Put(this.Value);
            }

            public static INetId<byte> Read(NetDataReader reader)
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
                if (other is NetId.Byte casted)
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
            public static INetId Zero { get; } = Create(0);

            public static byte SizeInBytes { get; } = 2;

            public ushort Value { get; init; }

            int INetId.Value => this.Value;

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