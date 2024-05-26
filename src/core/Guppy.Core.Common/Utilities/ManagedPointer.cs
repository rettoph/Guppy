using System.Runtime.InteropServices;

namespace Guppy.Core.Common.Utilities
{
    public struct ManagedPointer<TValue>
        where TValue : notnull
    {
        private readonly IntPtr _ptr;

        public TValue Value
        {
            get
            {
                GCHandle handle = GCHandle.FromIntPtr(_ptr);
                TValue value = (TValue?)handle.Target ?? throw new NotImplementedException();

                return value;
            }
        }

        public ManagedPointer()
        {
            throw new InvalidOperationException();
        }

        public ManagedPointer(TValue value)
        {
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            _ptr = GCHandle.ToIntPtr(handle);
        }

        public void Free()
        {
            GCHandle handle = GCHandle.FromIntPtr(_ptr);
            handle.Free();
        }

        public static implicit operator TValue(ManagedPointer<TValue> value)
        {
            return value.Value;
        }
    }
}
