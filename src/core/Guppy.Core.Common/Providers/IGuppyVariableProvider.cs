using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Common.Providers
{
    public interface IGuppyVariableProvider<TVariable>
        where TVariable : IGuppyVariable
    {
        bool TryGet<T>([MaybeNullWhen(false)] out T variable)
            where T : TVariable;

        IEnumerable<T> GetAll<T>()
            where T : TVariable;
    }
}
