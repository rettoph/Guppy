using Autofac;
using Guppy.Common;

namespace Guppy
{
    internal class Optional<T> : IOptional<T>
        where T : class
    {
        public T? Value { get; }

        public Optional(ILifetimeScope scope)
        {
            this.Value = scope.ResolveOptional<T>();
        }
    }
}
