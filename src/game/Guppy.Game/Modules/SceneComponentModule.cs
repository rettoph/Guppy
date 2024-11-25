using Autofac;
using Guppy.Core.Common.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Extensions;

namespace Guppy.Game.Modules
{
    public class SceneComponentModule(IAssemblyService assemblyService) : Module
    {
        private readonly IAssemblyService _assemblyService = assemblyService;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            foreach (Type sceneType in _assemblyService.GetTypes<IScene>())
            {
                Type sceneComponentType = typeof(ISceneComponent<>).MakeGenericType(sceneType);

                builder.RegisterSceneFilter(sceneComponentType, sceneType);
            }
        }
    }
}
