using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Utilities.Factories;

namespace Guppy.Collections
{
    public class LayerCollection : FactoryCollection<Layer>
    {
        public LayerCollection(PooledFactory<Layer> factory, IServiceProvider provider) : base(factory, provider)
        {
        }
    }
}
