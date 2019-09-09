using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    /// <summary>
    /// Represents a type that will automatically be registered as a service.
    /// </summary>
    public class IsServiceAttribute : GuppyAttribute
    {
        public readonly ServiceLifetime Lifetime;

        public IsServiceAttribute(ServiceLifetime lifetime)
        {
            this.Lifetime = lifetime;
        }
    }
}
