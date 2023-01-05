﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IGlobal<T>
        where T : notnull
    {
        T Instance { get; }
    }
}