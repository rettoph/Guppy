using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Constants;
using Guppy.Game.Common.Services;

namespace Guppy.Game.Services
{
    internal sealed class SceneService(IGuppyScope scope, IConfigurationService configurations) : ISceneService
    {
        private readonly IGuppyScope _scope = scope;
        private readonly List<IScene> _scenes = [];
        private readonly Dictionary<IScene, IGuppyScope> _scopes = [];
        private readonly IConfigurationService _configurations = configurations;

        public event OnEventDelegate<ISceneService, IScene>? OnSceneCreated;
        public event OnEventDelegate<ISceneService, IScene>? OnSceneDestroyed;

        public T Create<T>(Action<IGuppyScopeBuilder>? buildDelegate)
            where T : class, IScene
        {
            ISceneConfiguration configuration = this.GetConfiguration(typeof(T));
            IGuppyScope scope = this._scope.CreateChildScope(builder =>
            {
                builder.AddScopeVariable(GuppyGameVariables.Scope.SceneType.Create(typeof(T)));

                builder.RegisterInstance(configuration);

                builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().SingleInstance();

                buildDelegate?.Invoke(builder);
            });

            T scene = scope.Resolve<T>();

            this.Configure(scene, scope);

            return scene;
        }

        public IScene Create(Type sceneType, Action<IGuppyScopeBuilder>? buildDelegate)
        {
            ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);

            ISceneConfiguration configuration = this.GetConfiguration(sceneType);
            IGuppyScope scope = this._scope.CreateChildScope(builder =>
            {
                builder.AddScopeVariable(GuppyGameVariables.Scope.SceneType.Create(sceneType));

                builder.RegisterInstance(configuration);

                builder.RegisterType(sceneType).AsSelf().AsImplementedInterfaces().SingleInstance();

                buildDelegate?.Invoke(builder);
            });
            IScene scene = scope.Resolve(sceneType) as IScene ?? throw new NotImplementedException();

            this.Configure(scene, scope);

            return scene;
        }

        public void Destroy(IScene guppy)
        {
            if (this._scopes.Remove(guppy, out var scope))
            {
                scope.Dispose();
            }

            this._scenes.Remove(guppy);
            this.OnSceneDestroyed?.Invoke(this, guppy);
        }

        private void Configure(IScene scenes, IGuppyScope scope)
        {
            scenes.Initialize(scope);
            this._scopes.Add(scenes, scope);
            this._scenes.Add(scenes);

            this.OnSceneCreated?.Invoke(this, scenes);
        }

        public void Dispose()
        {
            this._scope.Dispose();

            while (this._scenes.Count != 0)
            {
                this.Destroy(this._scenes.First());
            }
        }

        public IEnumerable<IScene> GetAll()
        {
            return this._scenes;
        }

        private SceneConfiguration GetConfiguration(Type sceneType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);

            Type sceneConfigurationType = typeof(SceneConfiguration<>).MakeGenericType(sceneType);
            SceneConfiguration configuration = (SceneConfiguration)(Activator.CreateInstance(sceneConfigurationType) ?? throw new NotImplementedException());

            this._configurations.Configure(configuration);

            return configuration;
        }
    }
}