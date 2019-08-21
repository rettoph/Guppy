using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    public class Loader<THandle, TValueIn, TValueOut> : ILoader
    {
        #region Structs
        protected struct RegisteredValues
        {
            public THandle Handle;
            public TValueIn Value;
            public UInt16 Priority;
        }

        #endregion

        #region Private Fields
        private Boolean _loaded;

        #endregion

        #region Protected Attributes
        protected ILogger logger { get; private set; }
        protected Dictionary<THandle, TValueOut> valuesTable { get; private set; }
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
        protected virtual void Register(THandle handle, TValueIn value, UInt16 priority = 100)
        {
            if (_loaded)
                throw new Exception($"Unable to register new value<{typeof(TValueIn).Name}> to key<{typeof(THandle).Name}>! Loader already loaded. Please register loader assets in a IServiceLoader class.");


            this.registeredValuesList.Add(new RegisteredValues()
            {
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

            this.valuesTable = this.BuildValuesTable();

            _loaded = true;

            this.logger.LogDebug($"Done. {this.valuesTable.Count} values cached.");
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
                    elementSelector: rv => this.GetOutValue(rv.Value));
        }

        protected virtual TValueOut GetOutValue(TValueIn value)
        {
            return (TValueOut)Convert.ChangeType(value, typeof(TValueOut));
        }

        public virtual TValueOut GetValue(THandle handle)
        {
            if (handle == null)
                return default(TValueOut);
            if (this.valuesTable.ContainsKey(handle))
                return this.valuesTable[handle];

            return default(TValueOut);
        }

        /// <summary>
        /// Manually set a loader value.
        /// This is only accessible after the loader
        /// is loaded and should be used as little as 
        /// possible.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="value"></param>
        public void SetValue(THandle handle, TValueOut value)
        {
            this.valuesTable[handle] = value;
        }
        #endregion
    }
}
