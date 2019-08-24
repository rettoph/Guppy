﻿using Guppy.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    public abstract class Loader<THandle, TIn, TOut> : ILoader
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
            get { return this.values[handle]; }
        }
        #endregion

        public Loader(ILogger logger)
        {
            _registeredValues = new List<RegisteredValue>();

            this.logger = logger;
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
            this.logger.LogTrace($"{this.GetType().Name} => Registering '{value}' under '{handle}' with priority {priority}.");

            _registeredValues.Add(new RegisteredValue()
            {
                Handle = handle,
                Value = value,
                Priority = priority
            });
        }
        #endregion
    }
}
