using Autofac;
using Guppy.Core.Common.Services;

namespace Guppy.Game.Modules
{
    public class SceneComponentModule(IAssemblyService assemblyService) : Module
    {
        private readonly IAssemblyService _assemblyService = assemblyService;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);


        }
    }
}