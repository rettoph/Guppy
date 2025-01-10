namespace Guppy.Game.ImGui.Common
{
    public class TrueTypeFont(byte[] data)
    {
        public byte[] Data { get; } = data;

        internal unsafe IntPtr GetDataPtr()
        {
            fixed (byte* ptr = this.Data)
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