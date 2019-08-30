using Guppy.Pooling;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities;
using Guppy.Pooling.Interfaces;
using Microsoft.Extensions.Logging;

namespace Guppy.Factories
{
    /// <summary>
    /// A factory that can create instances of Creatable objects.
    /// 
    /// This will automatically pool them and run TryCreate when
    /// they are first created.
    /// </summary>
    /// <typeparam name="TCreatable"></typeparam>
    public class CreatableFactory<TCreatable> : Factory<TCreatable>
        where TCreatable : Creatable
    {
        #region Constructor
        public CreatableFactory(IPoolManager<TCreatable> pools, IServiceProvider provider) : base(pools, provider)
        {
        }
        #endregion

        #region Build Methods
        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            return base.Build<T>(
                provider: provider,
                pool: pool,
                setup: setup,
                create: creatable =>
                {
                    create?.Invoke(creatable);
                    creatable.TryCreate(provider);
                });
        }
        #endregion
    }
}
