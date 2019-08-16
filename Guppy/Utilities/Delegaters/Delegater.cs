using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
    public class Delegater<TKey, TArg>
    {
        #region Delegates
        public delegate void CustomDelegater<TCustomArg>(Object sender, TCustomArg arg)
            where TCustomArg : TArg;
        #endregion

        #region Private Fields
        private ILogger _logger;
        private Dictionary<TKey, Type> _registeredDelegates;
        #endregion

        #region Constructor
        public Delegater(ILogger logger)
        {
            _logger = logger;
            _registeredDelegates = new Dictionary<TKey, Type>();
        }
        #endregion

        #region Register Methods
        public void TryRegister(TKey key)
        {
            this.TryRegister<TArg>(key);
        }
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
        private void Register<TCustomArg>(TKey key)
            where TCustomArg : TArg
        {
            if (_registeredDelegates.ContainsKey(key))
                throw new Exception($"Unable to register delegate '{key}' with Type<{typeof(TCustomArg).Name}>. Another delegate with this key has already been registered.");

            // Save the key...
            _registeredDelegates.Add(key, typeof(TCustomArg));
        }
        #endregion

        #region Add Methods
        #endregion

        #region Validate Methods
        private Boolean ValidateDelegateType(TKey key, Type type)
        {
            // Validate the requested delegate...
            if (!_registeredDelegates.ContainsKey(key))
                _logger.LogError($"Unable to validate delegate. Unknown key '{key}'.");
            else if (_registeredDelegates[key] != type)
                _logger.LogError($"Unable to validate delegate. Improper type defined. Expected {_registeredDelegates[key].Name} but recieved '{type.Name}'.");
            else
                return true;

            return false;
        }
        #endregion
    }
}
