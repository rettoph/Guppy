using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    /// <summary>
    /// Simple attribute that may be applied to IService
    /// classes to automatically register them,
    /// disregarding the need for a custom service 
    /// provider.
    /// </summary>
    public sealed class ServiceAttribute : AutoLoadAttribute
    {
        public readonly String Handle;
        public readonly Lifetime Lifetime;
        public readonly Type BaseType;

        public ServiceAttribute(String handle, Lifetime lifetime = Lifetime.Transient, Type baseType = null, Int32 priority = 100) : this(lifetime, baseType, priority)
        {
            this.Handle = handle;
        }
        public ServiceAttribute(Lifetime lifetime = Lifetime.Transient, Type baseType = null, Int32 priority = 100) : base(priority)
        {
            this.Lifetime = lifetime;
            this.BaseType = baseType;
        }
    }
}
