using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Factories
{
    public class SceneFactory : DrivenFactory<Scene>
    {
        #region Constructor
        public SceneFactory(DriverLoader drivers, IPoolManager<Scene> pools, IServiceProvider provider) : base(drivers, pools, provider)
        {
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            var options = provider.GetRequiredService<ScopeOptions>();

            // If there is not an existing scene yet, create one
            if (options.Scene == null)
            {
                var scope = provider.CreateScope().ServiceProvider;
                return base.Build<T>(
                    provider: scope,
                    pool: pool, 
                    setup: scene =>
                    {
                        // Update the scene's scope...
                        scope.GetService<ScopeOptions>().Scene = scene;
                        // Run any recieved custom setup methods...
                        setup?.Invoke(scene);
                    },
                    create: create);
            }
            else
            {
                return options.Scene as T;
            }
        }
    }
}
