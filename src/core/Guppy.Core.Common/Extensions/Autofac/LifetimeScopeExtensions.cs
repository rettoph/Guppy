using Autofac;

namespace Guppy.Core.Common.Extensions.Autofac
{
    public static class LifetimeScopeExtensions
    {
        public static bool HasTag(this ILifetimeScope lifetimeScope, object tag) => lifetimeScope.Resolve<ITags>().Has(tag);

        public static bool IsTag(this ILifetimeScope lifetimeScope, object tag) => lifetimeScope.Tag.Equals(tag);

        public static bool IsRoot(this ILifetimeScope lifetimeScope)
        {
            ITags tags = lifetimeScope.Resolve<ITags>();
            return tags.IsRoot;
        }
    }
}