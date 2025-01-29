using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Implementations;
using Guppy.Core.Common.Services;

namespace Guppy.Core
{
    public class FilteredGuppyScopeBuilder(GuppyScopeBuilder builder) : IGuppyScopeBuilder
    {
        private readonly GuppyScopeBuilder _builder = builder;

        public IEnvironmentVariableService EnvironmentVariables => this._builder.EnvironmentVariables;

        public IGuppyScope? ParentScope => this._builder.ParentScope;

        public ContainerBuilder ContainerBuilder => this._builder.ContainerBuilder;

        public IGuppyScopeBuilder AddScopeVariable(IScopeVariable variable)
        {
            return this._builder.AddScopeVariable(variable);
        }

        public IGuppyScopeBuilder AddScopeVariable<TKey, TValue>(TValue value)
            where TKey : IScopeVariable<TKey, TValue>
            where TValue : notnull
        {
            return this._builder.AddScopeVariable<TKey, TValue>(value);
        }

        public IGuppyScope Build()
        {
            return this._builder.Build();
        }

        public IGuppyScopeBuilder Filter(Action<GuppyScopeFilterBuilder> filterBuilder, Action<IGuppyScopeBuilder> scopeBuilder)
        {
            GuppyScopeFilterBuilder filterDelegateBuilder = new();
            filterBuilder(filterDelegateBuilder);
            GuppyScopeFilter scopeFilter = new(filterDelegateBuilder.Build(), scopeBuilder);

            return this._builder.Filter(scopeFilter, false);
        }

        public TVariable? GetScopeVariable<TVariable>() where TVariable : IScopeVariable
        {
            return this._builder.GetScopeVariable<TVariable>();
        }

        public T? GetVariable<T>() where T : IScopeVariable
        {
            return this.GetScopeVariable<T>();
        }
    }
}
