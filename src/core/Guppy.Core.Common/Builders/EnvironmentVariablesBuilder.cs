using System.Reflection;
using Guppy.Core.Common.Constants;

namespace Guppy.Core.Common.Builders
{
    public class EnvironmentVariablesBuilder(
        string company,
        string name,
        Assembly? entry = null
    ) : GuppyVariablesBuilder<EnvironmentVariablesBuilder, IEnvironmentVariable>(new([
            GuppyVariables.Global.Company.Create(company),
            GuppyVariables.Global.Project.Create(name),
            GuppyVariables.Global.EntryAssembly.Create(entry ?? Assembly.GetEntryAssembly() ?? throw new NotImplementedException())
        ]))
    {
    }
}