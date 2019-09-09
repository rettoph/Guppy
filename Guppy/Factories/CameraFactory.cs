using Guppy.Pooling.Interfaces;
using Guppy.Utilities.Cameras;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    internal sealed class CameraFactory : CreatableFactory<Camera>
    {
        public CameraFactory(IPoolManager<Camera> pools, IServiceProvider provider) : base(pools, provider)
        {
        }
    }
}
