using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Game.Common.Extensions;

namespace Guppy.Game.Common.Attributes
{
    [Obsolete]
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
            builder.RegisterSceneFilter(classType, this.SceneType);
        }
    }

    [Obsolete]
    public class SceneFilterAttribute<TScene> : SceneFilterAttribute
        where TScene : IScene
    {
        public SceneFilterAttribute() : base(typeof(TScene)) { }
    }
}
