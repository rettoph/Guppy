using System.IO;

namespace Guppy.MonoGame.UI
{
    public class TrueTypeFont
    {
        private byte[] _data;

        public byte[] Data => _data;

        public TrueTypeFont(byte[] data)
        {
            _data = data;
        }

        public unsafe IntPtr GetDataPtr()
        {
            fixed(byte* ptr = _data)
            {
                return (IntPtr)ptr;
            }
        }

        public int GetDataSize()
        {
            return this.Data.Length * sizeof(byte);
        }
    }
}