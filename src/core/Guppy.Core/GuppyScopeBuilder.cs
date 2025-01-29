using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Services;
using Guppy.Core.Services;

namespace Guppy.Core
{
    public class GuppyScopeBuilder : IGuppyScopeBuilder
    {
        public IGuppyScope? ParentScope { get; }

        public ContainerBuilder ContainerBuilder { get; }

        public ScopeVariablesBuilder Variables { get; }

        public GuppyScopeBuilder(
            GuppyScopeTypeEnum type,
            IGuppyScope? parentScope,
            ContainerBuilder? builder = null)
        {
            this.ParentScope = parentScope;
            this.ContainerBuilder = builder ?? new ContainerBuilder();
            this.Variables = new([
                GuppyVariables.Scope.ScopeType.Create(type)
            ]);

            this.ContainerBuilder.Register<IGuppyScope>((IComponentContext context) => new GuppyScope(this.ParentScope, context.Resolve<ILifetimeScope>(), context.Resolve<IScopeVariableService>(), context.Resolve<IEnvironmentVariableService>()));
            this.ContainerBuilder.Register<IScopeVariableService>((IComponentContext context) => new ScopeVariableService(this.Variables.Build()));
        }

        public IGuppyScope Build()
        {
            IContainer container = this.ContainerBuilder.Build();
            return container.Resolve<IGuppyScope>();
        }
    }
}
