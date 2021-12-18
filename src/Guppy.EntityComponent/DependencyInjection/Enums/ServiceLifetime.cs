using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public enum ServiceLifetime
    {
        Singleton,
        Scoped,
        Transient
    }
}
