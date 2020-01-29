using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    public abstract class Loader<THandle, TIn, TOut> : ILoader, IEnumerable<KeyValuePair<THandle, TOut>>
    {
        public struct RegisteredValue
        {
            public THandle Handle { get; internal set; }
            public TIn Value { get; internal set; }
            public Int32 Priority { get; internal set; }
        }

        #region Private Fields
        private List<RegisteredValue> _registeredValues;
        #endregion

        #region Protected Attributes
        protected Dictionary<THandle, TOut> values { get; private set; }
        protected ILogger logger { get; private set; }
        #endregion

        #region Public Attributes
        public TOut this[THandle handle]
        {
            get {
                if (handle != null && this.values.ContainsKey(handle))
                    return this.values[handle];
                else
                    return default(TOut);
            }
        }
        #endregion

        public Loader(ILogger logger)
        {
            _registeredValues = new List<RegisteredValue>();

            this.logger = logger;
        }

        public Boolean ContainsKey(THandle key)
        {
            return this.values.ContainsKey(key);
        }

        #region ILoader Implementation
        public virtual void Load()
        {
            this.logger.LogTrace($"Loading Loader<{this.GetType().Name}>...");
            this.values = _registeredValues.GroupBy(rv => rv.Handle).ToDictionary(
                keySelector: g => g.Key,
                elementSelector: this.BuildOutput);

            this.logger.LogTrace($"{this.values.Count} values cached.");
        }
        #endregion

        #region Building
        protected abstract TOut BuildOutput(IGrouping<THandle, RegisteredValue> registeredValues);

        protected virtual void Register(THandle handle, TIn value, Int32 priority = 100)
        {
#if DEBUG
            this.logger.LogTrace($"{this.GetType().Name} => Registering '{value}' under '{handle}' with priority {priority}.");
#endif
            _registeredValues.Add(new RegisteredValue()
            {
                Handle = handle,
                Value = value,
                Priority = priority
            });
        }
        #endregion

        #region IEnumerable Implementation
        public IEnumerator<KeyValuePair<THandle, TOut>> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }
        #endregion
    }
}
