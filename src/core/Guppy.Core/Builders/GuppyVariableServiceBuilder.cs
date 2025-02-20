using System.ComponentModel;
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

        public T? Get<T>()
            where T : TVariable
        {
            return this._variables.OfType<T>().LastOrDefault();
        }

        public virtual IEnumerable<TVariable> GetAll()
        {
            return this._variables;
        }

        public abstract TService Build();
    }
}