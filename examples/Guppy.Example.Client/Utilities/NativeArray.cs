using System.Runtime.InteropServices;

namespace Guppy.Example.Client.Utilities
{
    public unsafe struct NativeArray<T> : IDisposable
        where T : unmanaged
    {
        public readonly int Length;

        private readonly T* _ptr;

        public ref T this[int index] => ref _ptr[index];

        public NativeArray(int length)
        {
            _ptr = (T*)Marshal.AllocHGlobal(sizeof(T) * length);

            this.Length = length;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal((nint)_ptr);
        }

        public Span<T> AsSpan()
        {
            return new Span<T>(_ptr, this.Length);
        }
    }
}
