﻿using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(IStateProvider state);
    }
}