﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IDriven : IFrameable
    {
        TDriver GetFirstDriver<TDriver>()
            where TDriver : class, IDriver;
        IEnumerable<IDriver> GetDrivers<TDriver>()
            where TDriver : class, IDriver;
    }
}
