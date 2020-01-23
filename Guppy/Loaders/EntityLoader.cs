using Guppy.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    /// <summary>
    /// Simple class used to register entity configurations at runtime.
    /// </summary>
    public sealed class EntityLoader : SimpleLoader<String, (Type type, Action<Object> setup)>
    {
        #region Private Fields
        private StringLoader _strings;
        #endregion

        #region Constructor
        public EntityLoader(StringLoader strings, ILogger logger) : base(logger)
        {
            _strings = strings;
        }
        #endregion

        #region Registration Methods
        public void TryRegister<TEntity>(String handle, Action<TEntity> setup, Int32 priority = 100)
            where TEntity : Entity
        {
            this.Register(
                handle, 
                (type: typeof(TEntity), setup: (entity) => setup(entity as TEntity)), 
                priority);
        }
        #endregion
    }
}
