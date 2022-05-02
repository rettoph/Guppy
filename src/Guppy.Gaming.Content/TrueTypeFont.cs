﻿using System.IO;

namespace Guppy.Gaming.Content
{
    public class TrueTypeFont
    {
        private byte[] _data;

        public byte[] Data => _data;

        public TrueTypeFont(byte[] data)
        {
            _data = data;
        }

        public unsafe byte* GetDataPtr()
        {
            fixed(byte* ptr = _data)
            {
                return ptr;
            }
        }

        public int GetDataSize()
        {
            return this.Data.Length * sizeof(byte);
        }
    }
}