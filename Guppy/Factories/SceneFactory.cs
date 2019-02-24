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
            var scopeConfiguration = provider.GetService(typeof(GameScopeConfiguration)) as GameScopeConfiguration;

            if(scopeConfiguration.Scene == null && !this.targetType.IsAbstract)
            { // Create a new scene...
                scopeConfiguration.Scene = ActivatorUtilities.CreateInstance(provider, this.targetType) as Scene;

                // Return our new scene
                return scopeConfiguration.Scene as TScene;
            }
            else if(this.targetType == scopeConfiguration.Scene.GetType() || this.targetType.IsAssignableFrom(scopeConfiguration.Scene.GetType()))
            { // Return the pre-existing scene of this type...
                return scopeConfiguration.Scene as TScene;
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
