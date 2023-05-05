using Guppy.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LiteNetLib.Utils
{
    public static class NetDataReaderExtensions
    {
        public static bool GetIf(this NetDataReader reader)
        {
            return reader.GetBool();
        }

        public static TEnum GetEnum<TEnum>(this NetDataReader reader)
            where TEnum : struct, Enum
        {
            var byteVal = reader.GetByte();
            return Unsafe.As<byte, TEnum>(ref byteVal);
        }

        public static Guid GetGuid(this NetDataReader reader)
        {
            byte[] buffer = new byte[16];
            reader.GetBytes(buffer, 16);

            return new Guid(buffer);
        }

        public static unsafe UInt128 GetUInt128(this NetDataReader reader)
        {
            UInt128 value = new UInt128();
            ulong* values = (ulong*)&value;

            values[0] = reader.GetULong();
            values[1] = reader.GetULong();

            return value;
        }
    }
}
