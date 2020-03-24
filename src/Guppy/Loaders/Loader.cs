using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Loaders
{
    public abstract class Loader
    {
        public abstract void Load(ServiceProvider provider);
    }
}
