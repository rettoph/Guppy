using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Common.Services;
using Guppy.Core.Services;
using Guppy.Core.Utilities;

namespace Guppy.Core.Builders
{
    public class GuppyRootBuilder : IGuppyRootBuilder
    {
        private readonly GuppyBuilderFilterManager<IGuppyRootBuilder> _rootFilters;

        public ContainerBuilder ContainerBuilder { get; }
        public IEnvironmentVariableServiceBuilder EnvironmentVariables { get; }

        public GuppyRootBuilder(IEnumerable<IEnvironmentVariable>? environmentVariables = null, ContainerBuilder? containerBuilder = null)
        {
            this._rootFilters = new GuppyBuilderFilterManager<IGuppyRootBuilder>();

            this.EnvironmentVariables = new EnvironmentVariableServiceBuilder(environmentVariables ?? Enumerable.Empty<IEnvironmentVariable>());
            this.ContainerBuilder = containerBuilder ?? new ContainerBuilder();

            // Configure
            this.ContainerBuilder.RegisterType<GuppyRoot>().AsSelf().As<IGuppyRoot>().As<IGuppyScope>().SingleInstance();
            this.ContainerBuilder.Register<IEnvironmentVariableService>(x => this.EnvironmentVariables.Build()).SingleInstance();
            this.ContainerBuilder.RegisterInstance<IScopeVariableService>(new ScopeVariableService(Enumerable.Empty<IScopeVariable>())).SingleInstance();
        }

        public IGuppyRoot Build()
        {
            this._rootFilters.ExcecuteAll(this);

            return this.ContainerBuilder.Build().Resolve<IGuppyRoot>();
        }

        public IGuppyRootBuilder Filter(Action<IGuppyContainerBuilderFilterBuilder<IGuppyRootBuilder>> filter, Action<IGuppyRootBuilder> build)
        {
            this._rootFilters.Add(filter, build);

            return this;
        }

        public IGuppyRootBuilder Filter(Action<IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder>> filter, Action<IGuppyScopeBuilder> build)
        {
            GuppyContainerBuilderFilter<IGuppyScopeBuilder> guppyContainerBuilderFilter = GuppyContainerBuilderFilter<IGuppyScopeBuilder>.Create(filter, build);
            this.RegisterInstance(guppyContainerBuilderFilter).SingleInstance();

            return this;
        }
    }
}
