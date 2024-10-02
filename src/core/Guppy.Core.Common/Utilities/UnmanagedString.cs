using System.Runtime.InteropServices;

namespace Guppy.Core.Common.Utilities
{
    public unsafe struct UnmanagedString : IDisposable
    {
        private readonly nint _ptr;
        private int _length;

        public readonly string Value => Marshal.PtrToStringAnsi(_ptr, _length);

        public UnmanagedString(string value)
        {
            if (value == string.Empty)
            {
                return;
            }

            _ptr = Marshal.StringToHGlobalAnsi(value);
            _length = value.Length;
        }

        public void Dispose()
        {
            if (_length == -1)
            {
                return;
            }

            _length = -1;
            Marshal.FreeHGlobal(_ptr);
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
