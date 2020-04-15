using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public enum ServiceLifetime
    {
        Singleton,
        Scoped,
        Transient
    }
}
