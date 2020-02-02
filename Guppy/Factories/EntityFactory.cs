using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities;
using Guppy.Extensions.Logging;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Guppy.Factories
{
    public sealed class EntityFactory : DrivenFactory<Entity>
    {
        #region Private Fields
        private EntityLoader _loader;
        #endregion

        #region Constructor
        public EntityFactory(EntityLoader loader, DriverLoader drivers, IPoolManager<Entity> pools, IServiceProvider provider) : base(drivers, pools, provider)
        {
            _loader = loader;
        }
        #endregion

        public TBase Build<TBase>(Type type, String handle, Action<TBase> setup = null, Action<TBase> create = null)
            where TBase : Entity
        {
            var loaded = _loader[handle];
            type = loaded.type == default(Type) ? type : loaded.type;

            // Ensure that the loaded type is an entity...
            ExceptionHelper.ValidateAssignableFrom<TBase>(type);

            this.logger.LogTrace(() => $"EntityFactory => Building Entity<{type.Name}>('{handle}') instance...");

            return this.Build<TBase>(type, e =>
            {
                // Update the handle...
                e.Handle = handle == default(String) ? type.Name : handle;
                e.Name = $"name:{e.Handle}";
                e.Description = $"description:{e.Handle}";

                // Run the auto setup if there is any...
                loaded.setup?.Invoke(e);
                // Run the custom setup if there is any...
                setup?.Invoke(e);
            }, create);
        }

        public TEntity Build<TEntity>(String handle, Action<TEntity> setup = null, Action<TEntity> create = null)
            where TEntity : Entity
        {
            return this.Build<TEntity>(typeof(TEntity), handle, setup, create);
        }

        public Entity Build(Type type, String handle, Action<Entity> setup = null, Action<Entity> create = null)
        {
            return this.Build<Entity>(type, handle, setup, create);
        }
    }
}
