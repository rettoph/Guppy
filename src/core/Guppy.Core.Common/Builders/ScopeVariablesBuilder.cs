namespace Guppy.Core.Common.Builders
{
    public class ScopeVariablesBuilder(List<IScopeVariable>? variables = null) : GuppyVariablesBuilder<ScopeVariablesBuilder, IScopeVariable>(variables)
    {
    }
}