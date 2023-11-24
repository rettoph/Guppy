﻿using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface ISettingProvider
    {
        Ref<T> Get<T>(Setting<T> setting)
            where T : notnull;

        void Set<T>(Setting<T> setting, T value)
            where T : notnull;
    }
}
