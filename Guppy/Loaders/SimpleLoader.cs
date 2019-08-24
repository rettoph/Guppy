using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Loaders
{
    /// <summary>
    /// Represents a loader that only cares about the priority of registered values.
    /// 
    /// No mutation is done on the in vs the out
    /// </summary>
    /// <typeparam name="THandle"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class SimpleLoader<THandle, TValue> : Loader<THandle, TValue, TValue>
    {
        public SimpleLoader(ILogger logger) : base(logger)
        {
        }

        protected override TValue BuildOutput(IGrouping<THandle, RegisteredValue> registeredValues)
        {
            return registeredValues.OrderBy(rv => rv.Priority).First().Value;
        }

        public virtual void TryRegister(THandle handle, TValue value, Int32 priority = 100)
        {
            this.Register(handle, value, priority);
        }
    }
}
