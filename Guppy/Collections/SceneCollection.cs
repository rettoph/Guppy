using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Utilities.Factories;

namespace Guppy.Collections
{
    public class SceneCollection : FactoryCollection<Scene>
    {
        public SceneCollection(PooledFactory<Scene> factory, IServiceProvider provider) : base(factory, provider)
        {
        }
    }
}
