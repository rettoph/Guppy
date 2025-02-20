using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Services;
using Guppy.Core.Utilities;

namespace Guppy.Core.Builders
{
    public class GuppyScopeBuilder : IGuppyScopeBuilder
    {
        private readonly GuppyBuilderFilterManager<IGuppyScopeBuilder> _scopeBuilderFilterManager;

        public IGuppyRoot Root { get; }

        public ContainerBuilder ContainerBuilder { get; }

        public IScopeVariableServiceBuilder Variables { get; }

        public GuppyScopeBuilder(
            IGuppyRoot root,
            IEnumerable<GuppyContainerBuilderFilter<IGuppyScopeBuilder>> scopeBuilderFilters,
            ContainerBuilder containerBuilder)
        {
            this._scopeBuilderFilterManager = new(scopeBuilderFilters);

            this.Root = root;
            this.ContainerBuilder = containerBuilder;
            this.Variables = new ScopeVariableServiceBuilder();

            this.ContainerBuilder.RegisterType<GuppyScope>().AsSelf().As<IGuppyScope>();
            this.ContainerBuilder.Register<IScopeVariableService>(x => this.Variables.Build()).InstancePerLifetimeScope();
        }

        public void ExcecuteFilters()
        {
            this._scopeBuilderFilterManager.ExcecuteAll(this);
        }

        public IGuppyScopeBuilder Filter(Action<IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder>> filter, Action<IGuppyScopeBuilder> build)
        {
            this._scopeBuilderFilterManager.Add(filter, build);

            return this;
        }
    }
}
