using System.Runtime.InteropServices;

namespace Guppy.Core.Common.Utilities
{
    public unsafe struct UnmanagedString : IDisposable
    {
        private readonly nint _ptr;
        private int _length;

        public readonly string Value => Marshal.PtrToStringAnsi(this._ptr, this._length);

        public UnmanagedString(string value)
        {
            if (value == string.Empty)
            {
                return;
            }

            this._ptr = Marshal.StringToHGlobalAnsi(value);
            this._length = value.Length;
        }

        public void Dispose()
        {
            if (this._length == -1)
            {
                return;
            }

            this._length = -1;
            Marshal.FreeHGlobal(this._ptr);
        }

        public static implicit operator string(UnmanagedString value)
        {
            return value.Value;
        }

        public static implicit operator UnmanagedString(string value)
        {
            return new UnmanagedString(value);
        }
    }
}