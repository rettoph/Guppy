using System.ComponentModel;

namespace Guppy.Core.Common.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class GuppyVariablesBuilder<TSelf, TVariable>
        where TSelf : GuppyVariablesBuilder<TSelf, TVariable>
        where TVariable : IGuppyVariable
    {
        private readonly List<TVariable> _variables;

        public GuppyVariablesBuilder(List<TVariable>? variables = null)
        {
            this._variables = variables ?? [];
        }

        public TSelf Add(TVariable variable)
        {
            this._variables.Add(variable);

            return (TSelf)this;
        }

        public T? Get<T>()
            where T : TVariable
        {
            return this._variables.OfType<T>().LastOrDefault();
        }

        public virtual IEnumerable<TVariable> Build()
        {
            return this._variables;
        }
    }
}