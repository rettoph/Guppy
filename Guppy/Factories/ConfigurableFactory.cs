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
using Guppy.Interfaces;

namespace Guppy.Factories
{
    public sealed class ConfigurableFactory<TConfigurable> : DrivenFactory<TConfigurable>
        where TConfigurable : IConfigurable
    {
        #region Private Fields
        private ConfigurationLoader _loader;
        #endregion

        #region Constructor
        public ConfigurableFactory(ConfigurationLoader loader, DriverLoader drivers, IPoolManager<TConfigurable> pools, IServiceProvider services) : base(drivers, pools, services)
        {
            _loader = loader;
        }
        #endregion

        public T Build<T>(Type type, String handle, Action<T> setup = null, Action<T> create = null)
            where T : TConfigurable
        {
            var loaded = _loader[handle];
            type = loaded.type == default(Type) ? type : loaded.type;

            // Ensure that the loaded type is an entity...
            ExceptionHelper.ValidateAssignableFrom<TConfigurable>(type);

            this.logger.LogTrace(() => $"EntityFactory => Building Entity<{type.Name}>('{handle}') instance...");

            return this.Build<T>(type, e =>
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

        public T Build<T>(String handle, Action<T> setup = null, Action<T> create = null)
            where T : TConfigurable
        {
            return this.Build<T>(typeof(T), handle, setup, create);
        }
    }
}
