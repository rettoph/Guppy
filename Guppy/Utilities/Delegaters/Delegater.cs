using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Delegaters
{
    /// <summary>
    /// Custom delegation utility, used to create and invoke
    /// custom events. Events can be added as instance focused
    /// </summary>
    public class Delegater<TKey, TArg> : IDisposable
    {
        #region Delegates
        public delegate void CustomDelegater<TCustomArg>(Object sender, TCustomArg arg)
            where TCustomArg : TArg;
        #endregion

        #region Private Fields
        private Object _owner;
        private Dictionary<TKey, Type> _registeredDelegates;
        private Dictionary<TKey, Delegate> _delegates;
        #endregion

        #region Protected Fields
        protected ILogger logger { get; private set; }
        #endregion

        #region Constructor
        public Delegater(ILogger logger)
        {
            _registeredDelegates = new Dictionary<TKey, Type>();
            _delegates = new Dictionary<TKey, Delegate>();
            this.SetOwner(123);

            this.logger = logger;
        }
        #endregion

        #region Lifecycle Methods
        public void Dispose()
        {
            // Clear all saved delegates
            _delegates.Clear();
        }
        #endregion

        #region Helper Methods
        internal void SetOwner(Object owner)
        {
            _owner = owner;
        }

        public void TryRegisterDelegate<TCustomArg>(TKey key)
        {
            if (_registeredDelegates.ContainsKey(key))
            {
                this.logger.LogWarning($"Unable to register delegate. Key already defined.");
                return;
            }

            // Store the delegate
            _registeredDelegates.Add(key, typeof(TCustomArg));
        }
        public void RegisterDelegate(TKey key)
        {
            this.TryRegisterDelegate<TArg>(key);
        }

        public void AddDelegate<TCustomArg>(TKey key, CustomDelegater<TCustomArg> d)
             where TCustomArg : TArg
        {
            if(this.ValidateDelegateType(key, typeof(TCustomArg)))
            {
                this.logger.LogDebug($"Adding new delegate for {_owner.GetType().Name}. Key => {key.GetType().Name}({key}), Arg => {typeof(TCustomArg).Name}");

                if (_delegates.ContainsKey(key))
                { // Add the delegate...
                    var delegates = (_delegates[key] as CustomDelegater<TCustomArg>);
                    delegates += d;
                    _delegates[key] = delegates;
                }
                else
                { // Save the delegate...
                    _delegates[key] = d;
                }
            }
        }
        public void AddDelegate(TKey key, CustomDelegater<TArg> d)
        {
            this.AddDelegate<TArg>(key, d);
        }

        public void RemoveDelegate<TCustomArg>(TKey key, CustomDelegater<TCustomArg> d)
            where TCustomArg : TArg
        {
            if(this.ValidateDelegateType(key, typeof(TCustomArg)))
            {
                this.logger.LogDebug($"Removing delegate for {_owner.GetType().Name}. Key => {key.GetType().Name}({key}), Arg => {typeof(TCustomArg).Name}");

                // Remove the delegate...
                var delegates = (_delegates[key] as CustomDelegater<TCustomArg>);
                delegates -= d;
                _delegates[key] = delegates;
            }
        }
        public void RemoveDelegate(TKey key, CustomDelegater<TArg> d)
        {
            this.RemoveDelegate<TArg>(key, d);
        }
        
        public void Invoke<TCustomArg>(TKey key, TCustomArg arg)
             where TCustomArg : TArg
        {
            this.ValidateDelegateType(key, typeof(TCustomArg));
            
            // Invoke the delegate...
            if(_delegates.ContainsKey(key))
                (_delegates[key] as CustomDelegater<TCustomArg>)?.Invoke(_owner, arg);
        }


        private Boolean ValidateDelegateType(TKey key, Type type)
        {
            // Validate the requested delegate...
            if (!_registeredDelegates.ContainsKey(key))
                this.logger.LogError($"Unable to validate delegate. Unknown key '{key}'.");
            else if (_registeredDelegates[key] != type)
                this.logger.LogError($"Unable to validate delegate. Improper type defined. Expected {_registeredDelegates[key].Name} but recieved '{type.Name}'.");
            else
                return true;

            return false;
        }
        #endregion
    }
}
