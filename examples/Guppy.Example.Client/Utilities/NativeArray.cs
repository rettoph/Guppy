using System.Runtime.InteropServices;

namespace Guppy.Example.Client.Utilities
{
    public readonly unsafe struct NativeArray<T>(int length) : IDisposable
        where T : unmanaged
    {
        public readonly int Length = length;

        private readonly T* _ptr = (T*)Marshal.AllocHGlobal(sizeof(T) * length);

        public readonly ref T this[int index] => ref _ptr[index];

        public readonly void Dispose()
        {
            Marshal.FreeHGlobal((nint)_ptr);
        }

        public readonly Span<T> AsSpan()
        {
            return new Span<T>(_ptr, this.Length);
        }
    }
}
