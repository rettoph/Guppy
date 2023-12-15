﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IOptional<T>
        where T : class
    {
        bool HasValue => Value is not null;
        public T? Value { get; }
    }
}
