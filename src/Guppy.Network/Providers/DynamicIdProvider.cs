using Guppy.Network.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    public class DynamicIdProvider
    {
        private ushort _count;
        private Func<ushort, DynamicId> _method;
        private Func<NetDataReader, ushort> _read;

        public DynamicIdProvider(ushort count)
        {
            _count = count;
            _method = DynamicIdProvider.GetFactoryMethod(count);
            _read = DynamicIdProvider.GetReadMethod(count);
        }

        public DynamicId Get(ushort value)
        {
            return _method(value);
        }

        public ushort Read(NetDataReader reader)
        {
            return _read(reader);
        }

        public IEnumerable<DynamicId> All()
        {
            for (ushort i = 0; i < _count; i++)
            {
                yield return this.Get(i);
            }
        }

        private static Func<NetDataReader, ushort> GetReadMethod(ushort count)
        {
            if (count <= byte.MaxValue)
            {
                return DynamicIdProvider.OneByteReadMethod;
            }

            return DynamicIdProvider.TwoByteReadMethod;
        }

        private static ushort OneByteReadMethod(NetDataReader reader)
        {
            return reader.GetByte();
        }

        private static ushort TwoByteReadMethod(NetDataReader reader)
        {
            byte[] buffer = new byte[2];
            reader.GetBytes(buffer, 2);
            return BitConverter.ToUInt16(buffer);
        }

        private static Func<ushort, DynamicId> GetFactoryMethod(ushort count)
        {
            if (count <= byte.MaxValue)
            {
                return DynamicIdProvider.OneByteFactoryMethod;
            }

            return DynamicIdProvider.TwoByteFactoryMethod;
        }

        private static DynamicId OneByteFactoryMethod(ushort value)
        {
            return new DynamicId(value, new[] { (byte)value });
        }

        private static DynamicId TwoByteFactoryMethod(ushort value)
        {
            return new DynamicId(value, BitConverter.GetBytes(value));
        }
    }
}
