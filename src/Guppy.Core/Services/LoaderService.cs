using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Enums;
using System.Linq;

namespace Guppy.Services
{
    /// <summary>
    /// Loaders represent Singleton services that should contain
    /// editable global assets & values. Examples include Colors,
    /// Strings, and Content.
    /// </summary>
    public abstract class LoaderService<THandle, TInValue, TOutValue> : Service
    {
        #region Structs
        public struct RegisteredValue
        {
            public THandle Handle { get; internal set; }
            public TInValue Value { get; internal set; }
            public Int32 Priority { get; internal set; }
        }
        #endregion

        #region Private Fields
        private HashSet<RegisteredValue> _registeredValues;
        private Dictionary<THandle, TOutValue> _values;
        #endregion

        #region Public Attributes
        public TOutValue this[THandle handle]
        {
            get => _values.ContainsKey(handle) ? _values[handle] : default(TOutValue);
            set => _values[handle] = value;
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _registeredValues = new HashSet<RegisteredValue>();
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            // In the post initialze phase we must configure the reigstered values by Handle.
            _values = _registeredValues.OrderBy(i => i.Priority).GroupBy(rv => rv.Handle)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => this.Configure(g));
        }
        #endregion

        #region Helper Methods
        public void TryRegister(THandle handle, TInValue value, Int32 priority = 0)
        {
            if (this.Status >= ServiceStatus.PreInitializing && this.Status < ServiceStatus.PostInitializing)
                _registeredValues.Add(new RegisteredValue()
                {
                    Handle = handle,
                    Value = value,
                    Priority = priority
                });
            else
                throw new InvalidOperationException("Unable to register value after Initialization.");
        }

        /// <summary>
        /// Configure a group of registered values and 
        /// return a converted TOutValue value.
        /// </summary>
        /// <param name="group"></param>
        protected abstract TOutValue Configure(IGrouping<THandle, RegisteredValue> group);
        #endregion
    }

    public abstract class LoaderService<THandle, TValue> : LoaderService<THandle, TValue, TValue>
    {
        protected override TValue Configure(IGrouping<THandle, RegisteredValue> group)
            => group.First().Value;
    }
}
