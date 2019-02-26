using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;

namespace Guppy.Loaders
{
    public class Loader<THandle, TValueIn, TValueOut> : ILoader
    {
        #region Structs
        protected struct RegisteredValues {
            public THandle Handle;
            public TValueIn Value;
            public UInt16 Priority;
        }

        #endregion

        #region Private Fields
        private Boolean _loaded;
        private Dictionary<THandle, TValueOut> _valuesTable;
        #endregion

        #region Protected Attributes
        protected ILogger logger { get; private set; }
        protected List<RegisteredValues> registeredValuesList { get; private set; }
        #endregion

        public TValueOut this[THandle handle]
        {
            get { return this.GetValue(handle); }
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
        /// <param name="handle"></param>
        /// <param name="value"></param>
        /// <param name="priority"></param>
        public virtual void Register(THandle handle, TValueIn value, UInt16 priority = 0)
        {
            if (_loaded)
                throw new Exception($"Unable to register new value<{typeof(TValueIn).Name}> to key<{typeof(THandle).Name}>! Loader already loaded.");


            this.registeredValuesList.Add(new RegisteredValues() {
                Handle = handle,
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
        protected virtual Dictionary<THandle, TValueOut> BuildValuesTable()
        {
            return this.registeredValuesList
                .GroupBy(rv => rv.Handle)
                .Select(g => g.OrderByDescending(rv => rv.Priority)
                    .FirstOrDefault())
                .ToDictionary(
                    keySelector: rv => rv.Handle,
                    elementSelector: rv => (TValueOut)Convert.ChangeType(rv.Value, typeof(TValueOut)));
        }

        public virtual TValueOut GetValue(THandle handle)
        {
            if(_valuesTable.ContainsKey(handle))
                return _valuesTable[handle];

            return default(TValueOut);
        }
        #endregion
    }
}
