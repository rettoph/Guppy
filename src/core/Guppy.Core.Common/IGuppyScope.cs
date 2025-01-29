using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Common
{
    public interface IGuppyScope : IDisposable
    {
        IGuppyScope? Parent { get; }
        IEnumerable<IGuppyScope> Children { get; }
        IEnvironmentVariableService EnvironmentVariables { get; }
        IScopeVariableService Variables { get; }

        IGuppyScope CreateChildScope(Action<IGuppyScopeBuilder>? builder);

        T ResolveService<T>()
            where T : notnull;

        object ResolveService(Type type);

        bool TryResolveService<T>([MaybeNullWhen(false)] out T? instance)
            where T : class;
    }
}
