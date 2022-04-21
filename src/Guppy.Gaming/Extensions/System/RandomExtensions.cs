using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class RandomExtensions
    {
        public static Vector2 NextVector2(this Random rand, Single min, Single max)
        {
            return rand.NextVector2(min, max, min, max);
        }
        public static Vector2 NextVector2(this Random rand, Single minX, Single maxX, Single minY, Single maxY)
        {
            return new Vector2(rand.NextSingle(minX, maxX), rand.NextSingle(minY, maxY));
        }

        public static Color NextColor(this Random rand, Byte? r = null, Byte? g = null, Byte? b = null, Byte? alpha = null)
        {
            Byte[] buffer = new Byte[4];
            rand.NextBytes(buffer);

            return new Color(r ?? buffer[0], g ?? buffer[1], b ?? buffer[2], alpha ?? buffer[3]);
        }
    }
}
