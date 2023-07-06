﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface ITypeProvider<T> : IEnumerable<Type>
    {
        IEnumerable<T> CreateInstances();
    }
}
