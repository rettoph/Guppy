using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Loaders;

namespace Guppy.Game.Input.Common.Loaders
{
    [AutoLoad]
    internal sealed class InputLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {

        }
    }
}
