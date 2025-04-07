using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Common.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class GuppyVariableServiceBuilder<TSelf, TVariable, TService> : IGuppyVariableServiceBuilder<TSelf, TVariable, TService>
        where TSelf : IGuppyVariableServiceBuilder<TSelf, TVariable, TService>
        where TVariable : IGuppyVariable
        where TService : IGuppyVariableService<TVariable>
    {
        private readonly List<TVariable> _variables;

        public GuppyVariableServiceBuilder(List<TVariable>? variables = null)
        {
            this._variables = variables ?? [];
        }

        public TSelf Add(TVariable variable)
        {
            if (this is not TSelf casted)
            {
                throw new NotImplementedException();
            }

            this._variables.Add(variable);

            return casted;
        }

        public bool TryGet<T>([MaybeNullWhen(false)] out T variable)
            where T : TVariable
        {
            variable = (T?)this._variables.LastOrDefault(x => x is T);
            return variable is not null;
        }

        public IEnumerable<T> GetAll<T>() where T : TVariable
        {
            return this._variables.Where(x => x is T).Select(x => (T)x);
        }

        public virtual IEnumerable<TVariable> GetAll()
        {
            return this._variables;
        }

        public abstract TService Build();
    }
}