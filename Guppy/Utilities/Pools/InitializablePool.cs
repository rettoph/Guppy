using Guppy.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    /// <summary>
    /// Pool implementation build specifically for Initializable types.
    /// This will automatically initiailze all pulled instances.
    /// </summary>
    public class InitializablePool : CreatablePool
    {
        public InitializablePool(Type targetType) : base(targetType)
        {
            if (!typeof(Initializable).IsAssignableFrom(targetType))
                throw new Exception($"Unable to create InitializablePool. TargetType must be assignable to Initializable. Input {targetType.Name} is not.");
        }

        public override T Pull<T>(IServiceProvider provider, Action<T> setup = null)
        {
            var instance = base.Pull(provider, setup);

            // Quickly initialize the instance
            var initializable = instance as Initializable;
            initializable.TryPreInitialize();
            initializable.TryInitialize();
            initializable.TryPostInitialize();

            return instance;
        }
    }
}
