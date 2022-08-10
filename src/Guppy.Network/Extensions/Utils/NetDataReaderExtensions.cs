using Guppy.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return Enum.GetValues<TEnum>().First(v => Convert.ToByte(v) == byteVal);
        }
    }
}
