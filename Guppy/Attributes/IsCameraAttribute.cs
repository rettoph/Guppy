using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class IsCameraAttribute : GuppyAttribute
    {
        public readonly ServiceLifetime Lifetime;

        public IsCameraAttribute(ServiceLifetime lifetime)
        {
            this.Lifetime = lifetime;
        }
    }
}
