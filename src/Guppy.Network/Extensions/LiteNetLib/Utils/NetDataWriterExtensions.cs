using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteNetLib.Utils
{
    public static class NetDataWriterExtensions
    {
        public static bool PutIf(this NetDataWriter writer, bool value)
        {
            writer.Put(value);

            return value;
        }

        public static void Put<TEnum>(this NetDataWriter writer, TEnum value)
            where TEnum : Enum
        {
            writer.Put(Convert.ToByte(value));
        }

        public static void Put(this NetDataWriter writer, Guid value)
        {
            writer.Put(value.ToByteArray());
        }

        public static unsafe void Put(this NetDataWriter writer, UInt128 value)
        {
            ulong* values = (ulong*)&value;

            writer.Put(values[0]);
            writer.Put(values[1]);
        }
    }
}
