using Autofac;
using Guppy.Core.Common;

namespace Guppy.Core
{
    internal class Optional<T>(ILifetimeScope scope) : IOptional<T>
        where T : class
    {
        public T? Value { get; } = scope.ResolveOptional<T>();
    }
}