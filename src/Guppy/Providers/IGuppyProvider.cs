using Autofac;
using Guppy.Common;
using Guppy.Common.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface IGuppyProvider : IEnumerable<IGuppy>, IDisposable
    {
        ILifetimeScope Scope { get; }

        event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyCreated;
        event OnEventDelegate<IGuppyProvider, IGuppy>? OnGuppyDestroyed;

        void Initialize();

        IGuppy Create(Type guppyType);

        T Create<T>()
            where T : class, IGuppy;
    }
}
