using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Services;
using Guppy.Core.Services;

namespace Guppy.Core.Builders
{
    public class EnvironmentVariableServiceBuilder : GuppyVariableServiceBuilder<IEnvironmentVariableServiceBuilder, IEnvironmentVariable, IEnvironmentVariableService>, IEnvironmentVariableServiceBuilder
    {
        public EnvironmentVariableServiceBuilder(IEnumerable<IEnvironmentVariable> environmentVariables)
        {
            foreach (IEnvironmentVariable environmentVariable in environmentVariables)
            {
                this.Add(environmentVariable);
            }
        }

        public override IEnvironmentVariableService Build()
        {
            return new EnvironmentVariableService(this.GetAll());
        }
    }
}
