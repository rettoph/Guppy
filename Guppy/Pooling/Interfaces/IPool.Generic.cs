using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Pooling.Interfaces
{
    public interface IPool<T>
    {
        Int32 Count();

        T Pull(Func<Type, T> factory);
        void Put(T instance);
    }
}
