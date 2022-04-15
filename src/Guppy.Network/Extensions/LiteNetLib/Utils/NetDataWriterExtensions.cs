using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteNetLib.Utils
{
    public static class NetDataWriterExtensions
    {
        public static Boolean PutIf(this NetDataWriter writer, Boolean value)
        {
            writer.Put(value);

            return value;
        }

        public static void Put<TEnum>(this NetDataWriter writer, TEnum value)
            where TEnum : Enum
        {
            writer.Put(Convert.ToByte(value));
        }
    }
}
