using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Extensions;
using Guppy.Game.Common.Services;

namespace Guppy.Game.Services
{
    internal sealed class SceneService(ILifetimeScope scope, IConfigurationService configurations) : ISceneService
    {
        private readonly ILifetimeScope _scope = scope;
        private readonly List<IScene> _scenes = [];
        private readonly Dictionary<IScene, ILifetimeScope> _scopes = [];
        private readonly IConfigurationService _configurations = configurations;

        public ILifetimeScope Scope => _scope;

        public event OnEventDelegate<ISceneService, IScene>? OnSceneCreated;
        public event OnEventDelegate<ISceneService, IScene>? OnSceneDestroyed;

        public T Create<T>(Action<ISceneConfiguration>? configurator, Func<ILifetimeScope, T>? factory)
            where T : class, IScene
        {
            ISceneConfiguration configuration = this.GetConfiguration(typeof(T));
            configurator?.Invoke(configuration);

            ILifetimeScope scope = _scope.BeginLifetimeScope(builder =>
            {
                builder.RegisterInstance(configuration);

                if (factory is null)
                {
                    builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().SingleInstance();
                }
                else
                {
                    builder.Register(factory).AsSelf().AsImplementedInterfaces().SingleInstance();
                }

                configuration.GetContainerBuilder()?.Invoke(builder);
            });
            T scene = scope.Resolve<T>();

            this.Configure(scene, scope);

            return scene;
        }

        public IScene Create(Type sceneType, Action<ISceneConfiguration>? configurator, Func<ILifetimeScope, object>? factory)
        {
            ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);

            ISceneConfiguration configuration = this.GetConfiguration(sceneType);
            configurator?.Invoke(configuration);


            ILifetimeScope scope = _scope.BeginLifetimeScope(builder =>
            {
                builder.RegisterInstance(configuration);

                if (factory is null)
                {
                    builder.RegisterType(sceneType).AsSelf().AsImplementedInterfaces().SingleInstance();
                }
                else
                {
                    builder.Register(factory).As(sceneType).As(sceneType.GetInterfaces()).SingleInstance();
                }

                configuration.GetContainerBuilder()?.Invoke(builder);
            });
            IScene scene = scope.Resolve(sceneType) as IScene ?? throw new NotImplementedException();

            this.Configure(scene, scope);

            return scene;
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

        private ISceneConfiguration GetConfiguration(Type sceneType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);

            Type sceneConfigurationType = typeof(SceneConfiguration<>).MakeGenericType(sceneType);
            SceneConfiguration configuration = (SceneConfiguration)(Activator.CreateInstance(sceneConfigurationType) ?? throw new NotImplementedException());

            _configurations.Configure(configuration);

            return configuration;
        }
    }
}
