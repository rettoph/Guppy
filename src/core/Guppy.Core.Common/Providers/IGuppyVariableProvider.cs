namespace Guppy.Core.Common.Providers
{
    public interface IGuppyVariableProvider<TVariable>
        where TVariable : IGuppyVariable
    {
        T? GetVariable<T>()
            where T : TVariable;
    }
}
