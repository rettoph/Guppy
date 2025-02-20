using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Constants;
using Guppy.Game.Common.Services;

namespace Guppy.Game.Services
{
    public class SceneService(IGuppyRoot root, IConfigurationService configurations) : ISceneService
    {
        private readonly IGuppyRoot _root = root;
        private readonly List<IScene> _scenes = [];
        private readonly IConfigurationService _configurations = configurations;

        public T Create<T>(Action<IGuppyScopeBuilder>? buildDelegate)
            where T : class, IScene
        {
            ISceneConfiguration configuration = this.GetConfiguration(typeof(T));
            IGuppyScope scope = this._root.CreateScope(builder =>
            {
                builder.Variables.Add(GuppyGameVariables.Scope.SceneType.Create(typeof(T)));

                builder.RegisterInstance(configuration);

                builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().SingleInstance();

                buildDelegate?.Invoke(builder);
            });
            T scene = scope.Resolve<T>();

            return scene;
        }

        public IScene Create(Type sceneType, Action<IGuppyScopeBuilder>? buildDelegate)
        {
            ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);

            ISceneConfiguration configuration = this.GetConfiguration(sceneType);
            IGuppyScope scope = this._root.CreateScope(builder =>
            {
                builder.Variables.Add(GuppyGameVariables.Scope.SceneType.Create(sceneType));

                builder.RegisterInstance(configuration);

                builder.RegisterType(sceneType).AsSelf().AsImplementedInterfaces().SingleInstance();

                buildDelegate?.Invoke(builder);
            });
            IScene scene = scope.Resolve(sceneType) as IScene ?? throw new NotImplementedException();

            return scene;
        }

        public void Destroy(IScene scene)
        {
            if (this.Remove(scene))
            {
                scene.Resolve<IGuppyScope>().Dispose();
            }
        }

        public void Add(IScene scene)
        {
            this._scenes.Add(scene);
        }

        public bool Remove(IScene scene)
        {
            return this._scenes.Remove(scene);
        }

        public void Dispose()
        {
            this._root.Dispose();

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