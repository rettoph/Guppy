using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Filters;

namespace Guppy.Game.Common.Attributes
{
    public class SceneFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly Type GameSceneType;

        public SceneFilterAttribute(Type gameSceneType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IScene>(gameSceneType);

            this.GameSceneType = gameSceneType;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new StateServiceFilter<Type>(classType, new State<Type>(
                key: StateKey<Type>.Create<IScene>(),
                value: this.GameSceneType)));
        }
    }

    public class SceneFilterAttribute<TGameScene> : SceneFilterAttribute
        where TGameScene : IScene
    {
        public SceneFilterAttribute() : base(typeof(TGameScene)) { }
    }
}
