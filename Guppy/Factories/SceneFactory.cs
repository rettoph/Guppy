using Guppy.Collections;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class SceneFactory<TScene> : Factory<TScene>
        where TScene : Scene
    {
        /// <summary>
        /// Create a new instance of the requested scene
        /// (if the scope does not alreadyy have a scene)
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public override TScene Create(IServiceProvider provider)
        {
            var config = provider.GetRequiredService<SceneScopeConfiguration>();

            if (config.Scene == null && !this.targetType.IsAbstract)
            { // Create a new scene...
                config.Scene = ActivatorUtilities.CreateInstance(provider, this.targetType) as TScene;

                return config.Scene as TScene;
            }
            else if (this.targetType == config.Scene.GetType() || this.targetType.IsAssignableFrom(config.Scene.GetType()))
            { // Return the pre-existing scene of this type...
                return config.Scene as TScene;
            }
            else
            { // Throw an error... the scope already has a scene of a different type...
                throw new Exception("Unable to create new Scene instance, scope contains another Scene.");
            }
        }

        public static SceneFactory<T> BuildFactory<T>()
            where T : Scene
        {
            return new SceneFactory<T>();
        }
    }
}
