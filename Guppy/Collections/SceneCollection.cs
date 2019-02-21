using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Collection containing all the current Game's Scenes
    /// Also can act as a factory for Scenes. Will automatically
    /// create scopes.
    /// </summary>
    public class SceneCollection : LivingObjectCollection<Scene>
    {
        private IServiceProvider _provider;

        public SceneCollection(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Create a new instance of a scene, complete with
        /// an internal scope.
        /// </summary>
        /// <typeparam name="TScene"></typeparam>
        /// <returns></returns>
        public TScene Create<TScene>()
            where TScene : Scene
        {
            // Create the new scene
            var scene = _provider.CreateScope().ServiceProvider.GetRequiredService<TScene>();

            // Add the new scene to the collection
            this.Add(scene);

            // return the new scene
            return scene;
        }
    }
}
