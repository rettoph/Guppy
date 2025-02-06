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

        T Resolve<T>()
            where T : notnull;

        T? ResolveOptional<T>()
            where T : class;

        object Resolve(Type type);

        bool TryResolve<T>([MaybeNullWhen(false)] out T? instance)
            where T : class;
    }
}
