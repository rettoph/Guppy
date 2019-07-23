using Guppy.Collections;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class SceneFactory : InitializableFactory<Scene>
    {
        /// <summary>
        /// Create a new instance of the requested scene
        /// (if the scope does not alreadyy have a scene)
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public override TScene Create<TScene>(IServiceProvider provider, params Object[] args)
        {
            var config = provider.GetRequiredService<SceneScopeConfiguration>();

            if (config.Scene == null && !this.targetType.IsAbstract)
            { // Create a new scene...
                config.Scene = base.Create<TScene>(provider, args);

                return config.Scene as TScene;
            }
            else if (this.targetType == config.Scene.GetType() || this.targetType.IsAssignableFrom(config.Scene.GetType()))
            { // Return the pre-existing scene of this type...
                return config.Scene as TScene;
            }
            else
            { // Throw an error... the scope already has a scene of a different type...
                throw new Exception("Unable to create new Scene instance, scope contains another Scene.");
            };
        }
    }
}
