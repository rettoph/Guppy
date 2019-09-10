using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Pooling.Interfaces;

namespace Guppy.Factories
{
    /// <summary>
    /// Factory object that will automatically
    /// initiailze any built objects before
    /// returning them.
    /// </summary>
    public class InitializableFactory<TInitializable> : CreatableFactory<TInitializable>
        where TInitializable : Initializable
    {
        #region Constructor
        public InitializableFactory(IPoolManager<TInitializable> pools, IServiceProvider provider) : base(pools, provider)
        {
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            var instance = base.Build(provider, pool, setup, create);

            instance.TryPreInitialize();
            instance.TryInitialize();
            instance.TryPostInitialize();

            return instance;
        }
    }
}
