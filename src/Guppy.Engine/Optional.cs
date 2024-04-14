using Autofac;
using Guppy.Engine.Common;

namespace Guppy.Engine
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
