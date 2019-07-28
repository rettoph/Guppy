using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.DynamicDelegaters
{
    /// <summary>
    /// Dynamic handlers are basically dictionary extensions
    /// that can be used to run methods based on a recieved key.
    /// 
    /// This functionality is seen within action handling, 
    /// custom events, and can be extended for any other 
    /// required event handling
    /// </summary>
    public abstract class DynamicDelegater<TKey, TParam> : IDisposable
    {
        private GlobalDynamicDelegte _globalDelegates;
        private Dictionary<TKey, LocalDynamicDelegate> _table;

        protected ILogger logger { get; private set; }

        public delegate void LocalDynamicDelegate(TParam arg);
        public delegate void GlobalDynamicDelegte(TKey key, TParam arg);

        public LocalDynamicDelegate this[TKey key] {
            get { return _table[key]; }
            set { _table[key] = value; }
        }

        public DynamicDelegater(ILogger logger)
        {
            // Create a new table to contain the delegates...
            _table = new Dictionary<TKey, LocalDynamicDelegate>();

            this.logger = logger;
        }

        /// <summary>
        /// Attempt to invoke a specific delegate based on a
        /// recieved key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public virtual Boolean TryInvoke(TKey key, TParam arg)
        {
            if (_table.ContainsKey(key))
            {
                _table[key].Invoke(arg);
                _globalDelegates?.Invoke(key, arg);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add a a handler function.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        public virtual void AddHandler(TKey key, LocalDynamicDelegate handler)
        {
            if (!_table.ContainsKey(key))
                _table[key] = handler;
            else
                _table[key] += handler;
        }

        /// <summary>
        /// Add a global handler that will be invoked for all events
        /// triggered by the current delegater
        /// </summary>
        /// <param name="handler"></param>
        public virtual void AddHandler(GlobalDynamicDelegte handler)
        {
            if (_globalDelegates == default(GlobalDynamicDelegte))
                _globalDelegates = handler;
            else
                _globalDelegates += handler;
        }

        /// <summary>
        /// Add a a handler function.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        public virtual void RemoveHandler(TKey key, LocalDynamicDelegate handler)
        {
            _table[key] -= handler;
        }

        public virtual void Dispose()
        {
            _table.Clear();
        }
    }
}
