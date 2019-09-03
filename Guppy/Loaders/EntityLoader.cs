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
        public void TryRegister<TEntity>(String handle, String nameHandle = "name:entity:default", String descriptionHandle = "description:entity:default", Object data = null, Int32 priority = 100)
            where TEntity : Entity
        {
            this.Register(handle, new EntityConfiguration()
            {
                Handle = handle,
                Name = nameHandle,
                Description = descriptionHandle,
                Data = data,
                Type = typeof(TEntity)
            }, priority);
        }

        protected override EntityConfiguration BuildOutput(IGrouping<string, RegisteredValue> registeredValues)
        {
            var config = base.BuildOutput(registeredValues);

            // Update the internal string values
            config.Name = _strings[config.Name];
            config.Description = _strings[config.Description];

            return config;
        }


        public override void TryRegister(String handle, EntityConfiguration value, Int32 priority = 100)
        {
            ExceptionHelper.ValidateAssignableFrom<Entity>(value.Type);

            base.Register(handle, value, priority);
        }
        #endregion
    }
}
