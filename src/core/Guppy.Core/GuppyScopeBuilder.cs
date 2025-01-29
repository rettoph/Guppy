using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Implementations;
using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;
using Guppy.Core.Services;

namespace Guppy.Core
{
    public class GuppyScopeBuilder : IGuppyScopeBuilder
    {
        private readonly ScopeVariablesBuilder _variables;
        private readonly List<IGuppyScopeFilter> _builders;

        public IGuppyScope? ParentScope { get; }

        public ContainerBuilder ContainerBuilder { get; }

        public IEnvironmentVariableService EnvironmentVariables { get; }

        public GuppyScopeBuilder(
            GuppyScopeTypeEnum type,
            IEnvironmentVariableService environmentVariableService,
            IGuppyScope? parentScope,
            ContainerBuilder? containerBuilder = null)
        {
            this._builders = [];
            this._variables = new([
                GuppyCoreVariables.Scope.ScopeType.Create(type)
            ]);

            this.ParentScope = parentScope;
            this.ContainerBuilder = containerBuilder ?? new ContainerBuilder();
            this.EnvironmentVariables = environmentVariableService;

            this.ContainerBuilder.Register<IGuppyScope>((IComponentContext context) => new GuppyScope(this.ParentScope, context.Resolve<ILifetimeScope>(), context.Resolve<IScopeVariableService>(), context.Resolve<IEnvironmentVariableService>()));
            this.ContainerBuilder.Register<IScopeVariableService>((IComponentContext context) => new ScopeVariableService(this._variables.Build()));

            if (this.ParentScope is not null)
            {
                foreach (IGuppyScopeFilter builder in this.ParentScope.ResolveService<IEnumerable<IGuppyScopeFilter>>())
                {
                    this.Filter(builder, true);
                }
            }
        }

        public IGuppyScope Build()
        {
            IContainer container = this.ContainerBuilder.Build();
            return container.Resolve<IGuppyScope>();
        }

        public IGuppyScopeBuilder AddScopeVariable(IScopeVariable variable)
        {
            this._variables.Add(variable);
            this.RunBuilders();

            return this;
        }

        public IGuppyScopeBuilder AddScopeVariable<TKey, TValue>(TValue value)
            where TKey : IScopeVariable<TKey, TValue>
            where TValue : notnull
        {
            this._variables.Add(TKey.Create(value));
            this.RunBuilders();

            return this;
        }

        public TVariable? GetScopeVariable<TVariable>()
            where TVariable : IScopeVariable
        {
            return this._variables.Get<TVariable>();
        }

        public IGuppyScopeBuilder Filter(Action<GuppyScopeFilterBuilder> filterBuilder, Action<IGuppyScopeBuilder> scopeBuilder)
        {
            GuppyScopeFilterBuilder filterDelegateBuilder = new();
            filterBuilder(filterDelegateBuilder);
            GuppyScopeFilter scopeFilter = new(filterDelegateBuilder.Build(), scopeBuilder);

            return this.Filter(scopeFilter, true);
        }

        public IGuppyScopeBuilder Filter(
            IGuppyScopeFilter filter,
            bool registerFilterAsService)
        {
            if (registerFilterAsService == true)
            {
                this.ContainerBuilder.RegisterInstance(filter).As<IGuppyScopeFilter>();
            }

            if (filter.Filter(this) == true)
            {
                FilteredGuppyScopeBuilder builder = new(this);
                filter.Build(builder);
                return this;
            }

            this._builders.Add(filter);
            return this;
        }

        private void RunBuilders()
        {
            for (int i = 0; i < this._builders.Count; i++)
            {
                IGuppyScopeFilter filter = this._builders[i];
                if (filter.Filter(this) == true)
                {
                    this._builders.RemoveAt(i);
                    i--;

                    // Ensure we run the filter build step AFTER
                    // removing it internally - this fixes a stack overflow
                    // recursion bug
                    FilteredGuppyScopeBuilder builder = new(this);
                    filter.Build(builder);
                }
            }
        }

        T? IGuppyVariableProvider<IScopeVariable>.GetVariable<T>() where T : default
        {
            return this.GetScopeVariable<T>();
        }
    }
}
