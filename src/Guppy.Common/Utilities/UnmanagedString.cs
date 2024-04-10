using Guppy.Common.Extensions;
using System.Runtime.InteropServices;

namespace Guppy.Common.Utilities
{
    public unsafe struct UnmanagedString : IDisposable
    {
        private readonly nint _ptr;
        private int _length;

        public string Value
        {
            get => Marshal.PtrToStringAnsi((nint)_ptr, _length);
            set
            {
                if (value.Length > _length)
                {
                    throw new NotImplementedException();
                }

                char* cPtr = (char*)_ptr;
                foreach ((char c, int i) in value.Indices())
                {
                    cPtr[i] = c;
                }

                _length = value.Length;
            }
        }

        public UnmanagedString(string value)
        {
            _ptr = Marshal.StringToHGlobalAnsi(value);
            _length = value.Length;
        }

        public void Dispose()
        {
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
