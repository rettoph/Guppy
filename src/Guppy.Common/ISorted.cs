﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface ISorted<T> : IEnumerable<T>
        where T : class
    {
    }
}