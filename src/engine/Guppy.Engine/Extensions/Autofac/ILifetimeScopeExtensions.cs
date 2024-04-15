using Autofac;
using Guppy.Core.Common;

namespace Guppy.Engine.Extensions.Autofac
{
    public static class ILifetimeScopeExtensions
    {
        public static ILifetimeScope BeginGuppyScope(this ILifetimeScope lifetimeScope, object tag, Action<ContainerBuilder>? build = null)
        {
            try
            {
                return lifetimeScope.BeginLifetimeScope(tag, builder =>
                {
                    build?.Invoke(builder);
                });
            }
            catch (Exception ex)
            {
                throw GuppyLogger.LogException($"Exception activating GuppyScope, Tag = {tag}", ex);
            }
        }

        public static bool HasTag(this ILifetimeScope lifetimeScope, object tag)
        {
            return lifetimeScope.Resolve<ITags>().Has(tag);
        }

        public static bool IsTag(this ILifetimeScope lifetimeScope, object tag)
        {
            return lifetimeScope.Tag.Equals(tag);
        }

        public static bool IsRoot(this ILifetimeScope lifetimeScope)
        {
            ITags tags = lifetimeScope.Resolve<ITags>();
            return tags.IsRoot;
        }
    }
}
