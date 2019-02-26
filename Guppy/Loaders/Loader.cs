using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;

namespace Guppy.Loaders
{
    public class Loader<TKey, TValueIn, TValueOut> : ILoader
    {
        #region Structs
        protected struct RegisteredValues {
            public TKey Key;
            public TValueIn Value;
            public UInt16 Priority;
        }

        #endregion

        #region Private Fields
        private Boolean _loaded;
        private Dictionary<TKey, TValueOut> _valuesTable;
        #endregion

        #region Protected Attributes
        protected ILogger logger { get; private set; }
        protected List<RegisteredValues> registeredValuesList { get; private set; }
        #endregion

        public TValueOut this[TKey key]
        {
            get { return this.GetValue(key); }
        }

        #region Constructors
        public Loader(ILogger logger)
        {
            this.logger = logger;
            this.registeredValuesList = new List<RegisteredValues>();

            _loaded = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Register a new asset to be loaded by the loader
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="priority"></param>
        public virtual void Register(TKey key, TValueIn value, UInt16 priority = 0)
        {
            if (_loaded)
                throw new Exception($"Unable to register new value<{typeof(TValueIn).Name}> to key<{typeof(TKey).Name}>! Loader already loaded.");


            this.registeredValuesList.Add(new RegisteredValues() {
                Key = key,
                Value = value,
                Priority = priority
            });
        }

        /// <summary>
        /// Load the registered values...
        /// Once this method is called no other values
        /// may be registered.
        /// </summary>
        public virtual void Load()
        {
            this.logger.LogDebug($"Loading Loader<{this.GetType().Name}>...");

            _valuesTable = this.BuildValuesTable();

            this.logger.LogDebug($"Done. {_valuesTable.Count} values loaded.");
        }

        /// <summary>
        /// Create a table containing processed values. This may be overwritten
        /// if more advanced processing is required beyond grabbing the highest valued
        /// object.
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<TKey, TValueOut> BuildValuesTable()
        {
            return this.registeredValuesList
                .GroupBy(rv => rv.Key)
                .Select(g => g.OrderByDescending(rv => rv.Priority)
                    .FirstOrDefault())
                .ToDictionary(
                    keySelector: rv => rv.Key,
                    elementSelector: rv => (TValueOut)Convert.ChangeType(rv.Value, typeof(TValueOut)));
        }

        public virtual TValueOut GetValue(TKey key)
        {
            return _valuesTable[key];
        }
        #endregion
    }
}
