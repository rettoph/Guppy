using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Utilities;

namespace Guppy.Core
{
    public class GuppyScopeBuilder : IGuppyScopeBuilder
    {
        public IGuppyScope? ParentScope { get; }

        public ContainerBuilder ContainerBuilder { get; }

        public Dictionary<Type, IEnvironmentVariable> EnvironmentVariables { get; }

        public GuppyScopeBuilder(
            Dictionary<Type, IEnvironmentVariable> environmentVariables,
            GuppyScopeTypeEnum type,
            IGuppyScope? parentScope,
            ContainerBuilder? builder = null)
        {
            this.EnvironmentVariables = new Dictionary<Type, IEnvironmentVariable>(environmentVariables);
            this.ParentScope = parentScope;
            this.ContainerBuilder = builder ?? new ContainerBuilder();

            this.ContainerBuilder.Register<IGuppyScope>((IComponentContext context) => new GuppyScope(this.ParentScope, type, context.Resolve<ILifetimeScope>()));
            this.ContainerBuilder.Register<GuppyEnvironment>((IComponentContext context) => new GuppyEnvironment(this.EnvironmentVariables));

        }

        public IGuppyScope Build()
        {
            IContainer container = this.ContainerBuilder.Build();
            return container.Resolve<IGuppyScope>();
        }
    }
}
