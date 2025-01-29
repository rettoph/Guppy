using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    public class ScopeVariableService(IEnumerable<IScopeVariable> variables) : GuppyVariableService<IScopeVariable>(variables), IScopeVariableService
    {
    }
}
