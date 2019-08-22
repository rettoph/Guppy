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
    public class Delegater<TKey, TArg> : IDisposable
    {
        #region Delegates
        public delegate void CustomDelegater<TCustomArg>(Object sender, TCustomArg arg)
            where TCustomArg : TArg;
        #endregion

        #region Private Fields
        private ILogger _logger;
        private Dictionary<TKey, Type> _registeredDelegates;
        private Dictionary<TKey, Delegate> _delegates;
        #endregion

        #region Constructor
        public Delegater(ILogger logger)
        {
            _logger = logger;
            _registeredDelegates = new Dictionary<TKey, Type>();
            _delegates = new Dictionary<TKey, Delegate>();
        }
        #endregion

        #region Lifecycle Methods 
        public void Dispose()
        {
            // Clear all saved delegates...
            _delegates.Clear();
            _delegates = _registeredDelegates.ToDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => (Delegate)null);
        }
        #endregion

        #region Register Methods
        /// <summary>
        /// Attempt to register a new delegate
        /// </summary>
        /// <param name="key"></param>
        public void TryRegister(TKey key)
        {
            this.TryRegister<TArg>(key);
        }
        /// <summary>
        /// Attempt to register a new delegate
        /// </summary>
        /// <param name="key"></param>
        public void TryRegister<TCustomArg>(TKey key)
            where TCustomArg : TArg
        {
            try
            {
                this.Register<TCustomArg>(key);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        public void Register<TCustomArg>(TKey key)
            where TCustomArg : TArg
        {
            if (_registeredDelegates.ContainsKey(key))
                throw new Exception($"Unable to register delegate '{key}' with Type<{typeof(TCustomArg).Name}>. Another delegate with this key has already been registered.");

            // Save the key...
            _logger.LogTrace($"Registering new delegate: Key<{typeof(TKey).Name}>('{key}'), Arg<{typeof(TCustomArg).Name}>");
            _registeredDelegates.Add(key, typeof(TCustomArg));
            _delegates.Add(key, null);
        }
        #endregion

        #region Add Methods
        /// <summary>
        /// Attempt to add a new delegate.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="d"></param>
        public void TryAdd(TKey key, CustomDelegater<TArg> d)
        {
            this.TryAdd<TArg>(key, d);
        }
        /// <summary>
        /// Attempt to add a new delegate.
        /// </summary>
        /// <typeparam name="TCustomArg"></typeparam>
        /// <param name="key"></param>
        /// <param name="d"></param>
        public void TryAdd<TCustomArg>(TKey key, CustomDelegater<TCustomArg> d)
            where TCustomArg : TArg
        {
            try
            {
                this.Add<TCustomArg>(key, d);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        public void Add<TCustomArg>(TKey key, CustomDelegater<TCustomArg> d)
            where TCustomArg : TArg
        {
            if(this.ValidateDelegateType(key, typeof(TCustomArg))) {
                if (_delegates[key] == null)
                { // Save the delegate...
                    _delegates[key] = d;
                }
                else
                { // Add the delegate...
                    var delegates = (_delegates[key] as CustomDelegater<TCustomArg>);
                    delegates += d;
                    _delegates[key] = delegates;
                }
            }
        }
        #endregion

        #region Invocation Methods
        /// <summary>
        /// Instantly invoke the delegate. No checks or validations are done.
        /// 
        /// This improves speed, but could cause crashes if a delegate is not
        /// defined or the given arg type is incorrect.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="key"></param>
        /// <param name="arg"></param>
        public void Invoke(TKey key, Object sender, TArg arg)
        {
            this.Invoke<TArg>(key, sender, arg);
        }
        /// <summary>
        /// Instantly invoke the delegate. No checks or validations are done.
        /// 
        /// This improves speed, but could cause crashes if a delegate is not
        /// defined or the given arg type is incorrect.
        /// </summary>
        /// <typeparam name="TCustomArg"></typeparam>
        /// <param name="sender"></param>
        /// <param name="key"></param>
        /// <param name="arg"></param>
        public void Invoke<TCustomArg>(TKey key, Object sender, TCustomArg arg)
            where TCustomArg : TArg
        {
            // Invoke the delegate...
            (_delegates[key] as CustomDelegater<TCustomArg>)?.Invoke(sender, arg);
        }
        #endregion

        #region Remove Methods
        /// <summary>
        /// Attempt to remove a delegate.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="d"></param>
        public void TryRemove(TKey key, CustomDelegater<TArg> d)
        {
            this.TryRemove<TArg>(key, d);
        }
        /// <summary>
        /// Attempt to remove a delegate.
        /// </summary>
        /// <typeparam name="TCustomArg"></typeparam>
        /// <param name="key"></param>
        /// <param name="d"></param>
        public void TryRemove<TCustomArg>(TKey key, CustomDelegater<TCustomArg> d)
            where TCustomArg : TArg
        {
            try
            {
                this.TryRemove<TCustomArg>(key, d);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        public void Remove<TCustomArg>(TKey key, CustomDelegater<TCustomArg> d)
            where TCustomArg : TArg
        {
            if (this.ValidateDelegateType(key, typeof(TCustomArg)))
            {
                if (_delegates.ContainsKey(key))
                { // Remove the delegate...
                    var delegates = (_delegates[key] as CustomDelegater<TCustomArg>);
                    delegates -= d;
                    _delegates[key] = delegates;
                }
            }
        }
        #endregion

        #region Validate Methods
        private Boolean ValidateDelegateType(TKey key, Type type)
        {
            // Validate the requested delegate...
            if (!_registeredDelegates.ContainsKey(key))
                throw new Exception($"Unable to validate delegate. Unknown key '{key}'.");
            else if (_registeredDelegates[key] != type)
                throw new Exception($"Unable to validate delegate. Improper type defined. Expected {_registeredDelegates[key].Name} but recieved '{type.Name}'.");
            else
                return true;
        }
        #endregion
    }
}
