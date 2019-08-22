using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Utilities.Factories;
using Microsoft.Extensions.Logging;

namespace Guppy.Collections
{
    public class SceneCollection : FrameableCollection<Scene>
    {
        private PooledFactory<Scene> _factory;

        public SceneCollection(PooledFactory<Scene> factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
        }

        #region Create Methods
        public TScene Create<TScene>(Action<TScene> setup = null)
            where TScene : Scene
        {
            var scene = _factory.Pull<TScene>(setup);
            this.Add(scene);
            return scene;
        }
        #endregion
    }
}
