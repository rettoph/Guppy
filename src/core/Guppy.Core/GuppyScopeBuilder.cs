using Autofac;
using Guppy.Core.Common;

namespace Guppy.Core
{
    public class GuppyScopeBuilder(IGuppyScope? parentScope, ContainerBuilder? builder = null) : IGuppyScopeBuilder
    {
        public IGuppyScope? ParentScope { get; private set; } = parentScope;

        public ContainerBuilder ContainerBuilder { get; private set; } = builder ?? new ContainerBuilder();
    }
}
