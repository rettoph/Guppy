using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Pooling.Interfaces;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Factories
{
    public class SceneFactory : InitializableFactory<Scene>
    {
        #region Constructor
        public SceneFactory(IPoolManager<Scene> pools, IServiceProvider provider) : base(pools, provider)
        {
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null)
        {
            var options = provider.GetRequiredService<ScopeOptions>();

            // If there is not an existing scene yet, create one
            if (options.Scene == null)
            {
                var scope = provider.CreateScope().ServiceProvider;
                return base.Build<T>(scope, pool, s =>
                {
                    // Update the scene's scope...
                    provider.GetService<ScopeOptions>().Scene = s;
                    // Run any recieved custom setup methods...
                    setup?.Invoke(s);
                });
            }
            else
            {
                return options.Scene as T;
            }
        }
    }
}
