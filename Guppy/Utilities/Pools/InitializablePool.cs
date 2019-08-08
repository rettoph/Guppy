using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class InitializablePool<TInitializable> : UniquePool<TInitializable>
        where TInitializable : class, IInitializable
    {
        public InitializablePool(Type targetType = null) : base(targetType)
        {
        }

        public override TInitializable Pull(IServiceProvider provider, Action<TInitializable> setup = null)
        {
            var child = base.Pull(provider, setup);

            // Auto initialize the child 
            child.TryPreInitialize();
            child.TryInitialize();
            child.TryPostInitialize();

            return child;
        }
    }
}
