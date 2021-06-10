using Guppy.DependencyInjection.TypeFactories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Actions
{
    /// <summary>
    /// Simple container for an action to be ran when creating
    /// a new type instance. These will be linked by an internal
    /// <see cref="Type"/> value & automatically
    /// applied to all inheriting type instances.
    /// </summary>
    public class BuilderAction : BaseAction<Type, ITypeFactory>
    {
        public BuilderAction(
            Type key, 
            Action<Object, ServiceProvider, ITypeFactory> method,
            Int32 order = 0) : base(key, method, order)
        {
        }
    }
}
