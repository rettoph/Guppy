using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class InitializableFactory<T> : Factory<T>
        where T : IInitializable
    {
        protected override Object Create(IServiceProvider provider, Type type, params object[] args)
        {
            var instance = base.Create(provider, type, args) as IInitializable;
            instance.TryBoot(); // Automatically boot the new instance...

            return instance;
        }
    }
}
