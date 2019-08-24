using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Pooling.Interfaces
{
    public interface IPoolManager<TBase>
    {
        IPool GetOrCreate<T>()
            where T : TBase;
        IPool GetOrCreate(Type type);
    }
}
