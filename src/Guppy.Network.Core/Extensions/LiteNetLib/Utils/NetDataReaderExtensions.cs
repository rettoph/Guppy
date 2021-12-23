using Guppy.Network.Structs;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteNetLib.Utils
{
    public static class NetDataReaderExtensions
    {
        public static Boolean GetIf(this NetDataReader reader)
        {
            return reader.GetBool();
        }

        public static TEnum GetEnum<TEnum>(this NetDataReader reader)
            where TEnum : Enum
        {
            var byteVal = reader.GetByte();
            return EnumHelper.GetValues<TEnum>().First(v => Convert.ToByte(v) == byteVal);
        }

        public static Vector2 GetVector2(this NetDataReader reader)
        {
            return new Vector2(reader.GetFloat(), reader.GetFloat());
        }
    }
}
