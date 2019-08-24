using Guppy.Configurations;
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
    public sealed class EntityLoader : SimpleLoader<String, EntityConfiguration>
    {
        #region Constructor
        public EntityLoader(ILogger logger) : base(logger)
        {
        }
        #endregion

        #region Registration Methods
        public void TryRegister<TEntity>(String handle, Object data = null, Int32 priority = 100)
            where TEntity : Entity
        {
            this.Register(handle, new EntityConfiguration()
            {
                Handle = handle,
                Data = data,
                Type = typeof(TEntity)
            }, priority);
        }

        public override void TryRegister(String handle, EntityConfiguration value, Int32 priority = 100)
        {
            ExceptionHelper.ValidateAssignableFrom<Entity>(value.Type);

            base.Register(handle, value, priority);
        }
        #endregion
    }
}
