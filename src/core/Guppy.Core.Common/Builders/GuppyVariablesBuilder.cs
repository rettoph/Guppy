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

        public virtual IEnumerable<TVariable> Build()
        {
            return this._variables;
        }
    }
}