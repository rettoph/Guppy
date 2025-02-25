﻿using System.Runtime.InteropServices;

namespace Guppy.Core.Common.Utilities
{
    public readonly struct ManagedPointer<TValue>
        where TValue : notnull
    {
        private readonly IntPtr _ptr;

        public readonly TValue Value
        {
            get
            {
                GCHandle handle = GCHandle.FromIntPtr(this._ptr);
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
            this._ptr = GCHandle.ToIntPtr(handle);
        }

        public readonly void Free()
        {
            GCHandle handle = GCHandle.FromIntPtr(this._ptr);
            handle.Free();
        }

        public static implicit operator TValue(ManagedPointer<TValue> value)
        {
            return value.Value;
        }
    }
}