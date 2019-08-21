using Guppy.Utilities;
using Guppy.Utilities.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class SceneCollection : FrameableCollection<Scene>
    {
        #region Private Fields
        private PooledFactory<Scene> _factory;
        #endregion

        #region Constructor
        public SceneCollection(PooledFactory<Scene> factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;

            this.Events.Register<Scene>("created");
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Create a new scene instance and add it to the collection
        /// </summary>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public Scene Create(Type sceneType, Action<Scene> setup = null)
        {
            return this.Create<Scene>(sceneType, setup);
        }
        /// <summary>
        /// Create a new scene instance and add it to the collection
        /// </summary>
        /// <typeparam name="TScene"></typeparam>
        /// <param name="setup"></param>
        /// <returns></returns>
        public TScene Create<TScene>(Action<TScene> setup = null)
            where TScene : Scene
        {
            return this.Create<TScene>(typeof(TScene), setup);
        }
        /// <summary>
        /// Create a new scene instance and add it to the collection
        /// </summary>
        /// <typeparam name="TScene"></typeparam>
        /// <param name="sceneType"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public TScene Create<TScene>(Type sceneType, Action<TScene> setup = null)
            where TScene : Scene
        {
            ExceptionHelper.ValidateAssignableFrom<TScene>(sceneType);

            // Create a new scene instance
            var scene = _factory.Pull<TScene>(setup);
            this.Events.Invoke<Scene>("created", this, scene);

            // Add the scene instance to the collection
            this.Add(scene);

            return scene;
        }
        #endregion
    }
}
