using Autofac;
using Guppy.Core.Common;
using Guppy.Game.Common;
using Guppy.Game.Common.Services;

namespace Guppy.Game.Services
{
    internal sealed class SceneService : ISceneService
    {
        private ILifetimeScope _scope;
        private List<IScene> _scenes;
        private Dictionary<IScene, ILifetimeScope> _scopes;

        public ILifetimeScope Scope => _scope;

        public event OnEventDelegate<ISceneService, IScene>? OnSceneCreated;
        public event OnEventDelegate<ISceneService, IScene>? OnSceneDestroyed;

        public SceneService(ILifetimeScope scope)
        {
            _scenes = new List<IScene>();
            _scope = scope;
            _scopes = new Dictionary<IScene, ILifetimeScope>();
        }

        public T Create<T>(Action<ContainerBuilder>? guppyBuilder)
            where T : class, IScene
        {
            var scope = _scope.BeginLifetimeScope(builder =>
            {
                builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().SingleInstance();
                guppyBuilder?.Invoke(builder);
            });
            var scene = scope.Resolve<T>();

            this.Configure(scene, scope);

            return scene;
        }

        public IScene Create(Type guppyType, Action<ContainerBuilder>? guppyBuilder)
        {
            ThrowIf.Type.IsNotAssignableFrom<IScene>(guppyType);

            var scope = _scope.BeginLifetimeScope(builder =>
            {
                builder.RegisterType(guppyType).AsSelf().AsImplementedInterfaces().SingleInstance();
                guppyBuilder?.Invoke(builder);
            });
            var guppy = scope.Resolve(guppyType) as IScene ?? throw new NotImplementedException();

            this.Configure(guppy, scope);

            return guppy;
        }

        public void Destroy(IScene guppy)
        {
            if (_scopes.Remove(guppy, out var scope))
            {
                scope.Dispose();
            }

            _scenes.Remove(guppy);
            this.OnSceneDestroyed?.Invoke(this, guppy);
        }

        private void Configure(IScene scenes, ILifetimeScope scope)
        {
            scenes.Initialize(scope);
            _scopes.Add(scenes, scope);
            _scenes.Add(scenes);

            this.OnSceneCreated?.Invoke(this, scenes);
        }

        public void Dispose()
        {
            _scope.Dispose();

            while (_scenes.Any())
            {
                this.Destroy(_scenes.First());
            }
        }

        public IEnumerable<IScene> GetAll()
        {
            return _scenes;
        }
    }
}
