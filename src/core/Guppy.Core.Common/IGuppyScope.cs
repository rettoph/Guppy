using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Common
{
    public interface IGuppyScope : IDisposable
    {
        IGuppyScope? Parent { get; }
        IEnumerable<IGuppyScope> Children { get; }

        bool IsRoot { get; }
        IGuppyScope Root { get; }

        IGuppyScope CreateChildScope(Action<IGuppyScopeBuilder> builder);

        T Resolve<T>()
            where T : notnull;

        object Resolve(Type type);

        bool TryResolve<T>([MaybeNullWhen(false)] out T? instance)
            where T : class;
    }
}
