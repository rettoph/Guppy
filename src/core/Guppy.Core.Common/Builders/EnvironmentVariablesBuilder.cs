using System.Reflection;
using Guppy.Core.Common.Constants;

namespace Guppy.Core.Common.Builders
{
    public class EnvironmentVariablesBuilder(
        string company,
        string name,
        Assembly? entry = null,
        IEnumerable<IEnvironmentVariable>? variables = null
    ) : GuppyVariablesBuilder<EnvironmentVariablesBuilder, IEnvironmentVariable>(new(
        (variables ?? Enumerable.Empty<IEnvironmentVariable>()).Concat([
            GuppyVariables.Global.Company.Create(company),
            GuppyVariables.Global.Project.Create(name),
            GuppyVariables.Global.EntryAssembly.Create(entry ?? Assembly.GetEntryAssembly() ?? throw new NotImplementedException())
        ])))
    {
        public static IEnumerable<IEnvironmentVariable> Build(string company, string name, Assembly? entry = null, IEnumerable<IEnvironmentVariable>? variables = null)
        {
            return new EnvironmentVariablesBuilder(company, name, entry, variables).Build();
        }
    }
}