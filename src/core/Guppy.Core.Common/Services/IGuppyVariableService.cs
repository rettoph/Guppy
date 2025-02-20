using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Common.Providers;

namespace Guppy.Core.Common.Services
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IGuppyVariableService<TVariable> : IGuppyVariableProvider<TVariable>
        where TVariable : IGuppyVariable
    {
        object Get(Type variableType);
        bool TryGet(Type variableType, [MaybeNullWhen(false)] out TVariable? value);
        bool Has(Type variableType);
        bool Matches(TVariable value);

        bool TryGet<T>([MaybeNullWhen(false)] out T? value)
            where T : TVariable;
        bool Has<T>()
            where T : TVariable;
        bool Matches<T>(T value)
            where T : TVariable;

        Dictionary<Type, TVariable> ToDictionary();
    }
}
