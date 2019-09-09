using Guppy.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public sealed class SceneCollection : CreatableCollection<Scene>
    {
        private SceneFactory _factory;

        public SceneCollection(SceneFactory factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
        }

        public TScene Create<TScene>(Action<TScene> setup = null)
            where TScene : Scene
        {
            var scene = _factory.Build<TScene>(setup);

            this.Add(scene);

            return scene;
        }
    }
}
