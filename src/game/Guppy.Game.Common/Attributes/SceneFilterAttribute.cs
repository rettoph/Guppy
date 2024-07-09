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
        public readonly Type? SceneType;

        public SceneFilterAttribute(Type? sceneType)
        {
            if (sceneType is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom<IScene>(sceneType);
            }

            this.SceneType = sceneType;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new StateServiceFilter<Type?>(classType, StateKey<Type?>.Create<IScene>(), this.SceneType));
        }
    }

    public class SceneFilterAttribute<TScene> : SceneFilterAttribute
        where TScene : IScene
    {
        public SceneFilterAttribute() : base(typeof(TScene)) { }
    }
}
