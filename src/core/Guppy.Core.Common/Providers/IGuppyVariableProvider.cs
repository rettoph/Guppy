namespace Guppy.Core.Common.Providers
{
    public interface IGuppyVariableProvider<TVariable>
        where TVariable : IGuppyVariable
    {
        T? Get<T>()
            where T : TVariable;
    }
}
