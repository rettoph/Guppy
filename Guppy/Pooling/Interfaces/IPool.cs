using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Pooling.Interfaces
{
    public interface IPool : IPool<Object>
    {
        Type TargetType { get; }
    }
}
