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
            ExceptionHelper.ValidateAssignableFrom<Initializable>(targetType);
        }

        public override T Pull<T>(Action<T> setup = null)
        {
            var instance = base.Pull(setup);

            // Quickly initialize the instance
            var initializable = instance as Initializable;
            initializable.TryPreInitialize();
            initializable.TryInitialize();
            initializable.TryPostInitialize();

            return instance;
        }
    }
}
