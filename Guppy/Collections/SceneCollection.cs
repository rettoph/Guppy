using Guppy.Enums;
using Guppy.Extensions;
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
            var scene = _provider.CreateScope().ServiceProvider.GetScene<TScene>();

            // Add the new scene to the collection
            this.Add(scene);

            // return the new scene
            return scene;
        }

        #region Collection Methods
        public override void Add(Scene item)
        {
            if (item.InitializationStatus != InitializationStatus.NotReady)
                throw new Exception($"Unable to add Scene too SceneCollection! Scene has been initialized.");

            base.Add(item);

            // When a new scene gets added, we must initialize it
            item.TryBoot();
            item.TryPreInitialize();
            item.TryInitialize();
            item.TryPostInitialize();
        }

        public override bool Remove(Scene item)
        {
            if(base.Remove(item))
            { // When a Scene gets removed, we must dispose of it...
                item.Dispose();

                return true;
            }

            return false;
        }
        #endregion
    }
}
