using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Factories
{
    public class SceneFactory : DrivenFactory<IScene>
    {
        #region Constructor
        public SceneFactory(DriverLoader drivers, IPoolManager<IScene> pools, IServiceProvider provider) : base(drivers, pools, provider)
        {
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            var options = provider.GetRequiredService<ScopeOptions>();

            // If there is not an existing scene yet, create one
            if (options.Get<IScene>() == null)
            {
                IServiceScope scope;
                IServiceProvider scopeProvider = (scope = provider.CreateScope()).ServiceProvider;
                return base.Build<T>(
                    provider: scopeProvider,
                    pool: pool, 
                    setup: scene =>
                    {
                        // Update the scene's scope...
                        scopeProvider.GetService<ScopeOptions>().Set<IScene>(scene);
                        scene.Scope = scope;
                        // Run any recieved custom setup methods...
                        setup?.Invoke(scene);
                    },
                    create: create);
            }
            else
            {
                return (T)options.Get<IScene>();
            }
        }
    }
}
