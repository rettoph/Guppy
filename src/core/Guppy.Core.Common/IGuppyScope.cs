using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Common
{
    public interface IGuppyScope : IDisposable, IGuppyVariableProvider<IScopeVariable>
    {
        IGuppyScope? Parent { get; }
        IEnumerable<IGuppyScope> Children { get; }
        IEnvironmentVariableService EnvironmentVariables { get; }
        IScopeVariableService Variables { get; }
        IScopedSystemService Systems { get; }

        IGuppyScope CreateChildScope(Action<IGuppyScopeBuilder>? builder);

        T ResolveService<T>()
            where T : notnull;

        T? ResolveOptionalService<T>()
            where T : class;

        object ResolveService(Type type);

        bool TryResolveService<T>([MaybeNullWhen(false)] out T? instance)
            where T : class;
    }
}
