using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Services;
using Guppy.Core.Services;

namespace Guppy.Core.Builders
{
    public class ScopeVariableServiceBuilder : GuppyVariableServiceBuilder<IScopeVariableServiceBuilder, IScopeVariable, IScopeVariableService>, IScopeVariableServiceBuilder
    {
        public override IScopeVariableService Build()
        {
            return new ScopeVariableService(this.GetAll());
        }
    }
}
