
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Delegaters
{
    /// <summary>
    /// Tool used for custom delegation.
    /// 
    /// Delegates can be defined using the
    /// TryRegisterDelegate method, in which a
    /// argument data type is defined.
    /// 
    /// After a delegate has been defined 
    /// </summary>
    public class EventDelegater : CustomDelegater<String, Object>
    {
        #region Private Fields
        private Dictionary<String, Type> _registeredDelegates;
        #endregion

        #region Constructor
        public EventDelegater()
        {
            _registeredDelegates = new Dictionary<String, Type>();
        }

        public void Register<T>(string v, object handlePointerScrolled)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Lifecycle Methods 
        public override void Dispose()
        {
            base.Dispose();

            // Clear all saved delegates...
            this.delegates = _registeredDelegates.ToDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => (Delegate)null);
        }
        #endregion

        #region Register Methods
        public void Register<TArg>(String key)
        {
            if (!_registeredDelegates.ContainsKey(key))
            {
                // Save the key...
                _registeredDelegates.Add(key, typeof(TArg));
                this.delegates.Add(key, null);
            }
            else
            {
                throw new Exception($"Event('{key}') has already been registered.");
            }
        }
        #endregion

        #region Add Methods
        public void TryAdd<T>(String key, CustomDelegate<T> d)
        {
            base.Add<T>(key, d);
        }
        protected override void Add<T>(String key, CustomDelegate<T> d)
        {
            if (this.ValidateArgType<T>(key))
                base.Add(key, d);
        }
        #endregion

        #region Remove Methods
        public void TryRemove<T>(String key, CustomDelegate<T> d)
        {
            base.Remove<T>(key, d);
        }
        protected override void Remove<T>(String key, CustomDelegate<T> d)
        {
            if (this.ValidateArgType<T>(key))
                base.Remove(key, d);
        }
        #endregion

        #region Invokation Methods
        public void TryInvoke<T>(Object sender, String key, T arg)
        {
            base.Invoke<T>(sender, key, arg);
        }
        #endregion

        #region Validation Methods
        public Boolean ValidateArgType<T>(String key)
        {
            if (!_registeredDelegates.ContainsKey(key))
                return false;
            if (typeof(T) != _registeredDelegates[key])
                return false;

            return true;
        }
        #endregion
    }
}
