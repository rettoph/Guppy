﻿using Guppy.Utilities.Delegaters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Factories
{
    public class PooledFactory<TBase>
        where TBase : class
    {
        #region Private Fields
        private PoolFactory _poolFactory;
        #endregion

        #region Constructor
        public PooledFactory(ILogger logger, PoolFactory poolFactory)
        {
            _poolFactory = poolFactory;
        }
        #endregion

        #region Helper Methods
        public TBase Pull(Type type, Action<TBase> setup = null)
        {
            var instance = _poolFactory.GetOrCreatePool(type).Pull<TBase>(setup);

            return instance;
        }
        public TInstance Pull<TInstance>(Action<TInstance> setup = null)
            where TInstance : class, TBase
        {
            var instance = _poolFactory.GetOrCreatePool(typeof(TInstance)).Pull<TInstance>(setup);

            return instance;
        }
        public TInstance Pull<TInstance>(Type type, Action<TInstance> setup = null)
            where TInstance : class, TBase
        {
            var instance = _poolFactory.GetOrCreatePool(type).Pull<TInstance>(setup);

            return instance;
        }
        #endregion
    }
}
