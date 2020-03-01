using Guppy.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    /// <summary>
    /// Simple class used to register custom initializable setups on runtime
    /// </summary>
    public sealed class ConfigurationLoader : SimpleLoader<String, (Type type, Action<Object> setup)>
    {
        #region Private Fields
        private StringLoader _strings;
        #endregion

        #region Constructor
        public ConfigurationLoader(StringLoader strings, ILogger logger) : base(logger)
        {
            _strings = strings;
        }
        #endregion

        #region Registration Methods
        public void TryRegister<TCreatable>(String handle, Action<TCreatable> setup, Int32 priority = 100)
            where TCreatable : Creatable
        {
            this.Register(
                handle, 
                (type: typeof(TCreatable), setup: (entity) => setup(entity as TCreatable)), 
                priority);
        }
        #endregion
    }
}
