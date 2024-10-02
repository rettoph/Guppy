namespace Guppy.Game.ImGui.Common
{
    public class TrueTypeFont(byte[] data)
    {
        private readonly byte[] _data = data;

        public byte[] Data => _data;

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
