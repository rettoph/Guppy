namespace Guppy.Game.ImGui
{
    public class TrueTypeFont
    {
        private byte[] _data;

        public byte[] Data => _data;

        public TrueTypeFont(byte[] data)
        {
            _data = data;
        }

        internal unsafe IntPtr GetDataPtr()
        {
            fixed (byte* ptr = _data)
            {
                return (IntPtr)ptr;
            }
        }

        internal int GetDataSize()
        {
            return this.Data.Length * sizeof(byte);
        }
    }
}
