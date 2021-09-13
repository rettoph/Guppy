using Guppy.DependencyInjection.TypeFactories;
using Guppy.Extensions.System;
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
            Action<Object, GuppyServiceProvider, ITypeFactory> method,
            Int32 order = 0,
            Func<IAction<Type, ITypeFactory>, Type, Boolean> filter = default) : base(key, method, order, filter)
        {
        }

        public override bool Filter(Type key)
        {
            return (this.Key.IsAssignableFrom(key)
                    || key.IsSubclassOf(this.Key)
                    || key.IsSubclassOfRawGeneric(this.Key)
                ) && base.Filter(key);
        }
    }
}
