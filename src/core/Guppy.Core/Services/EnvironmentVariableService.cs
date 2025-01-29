using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    public class EnvironmentVariableService(IEnumerable<IEnvironmentVariable> variables) : GuppyVariableService<IEnvironmentVariable>(variables), IEnvironmentVariableService
    {
    }
}
