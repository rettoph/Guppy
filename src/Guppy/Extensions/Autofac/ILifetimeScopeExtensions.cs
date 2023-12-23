using Autofac;
using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.Loaders;

namespace Guppy.Extensions.Autofac
{
    public static class ILifetimeScopeExtensions
    {
        public static ILifetimeScope BeginGuppyScope(this ILifetimeScope lifetimeScope, object tag, Action<ContainerBuilder>? build = null)
        {
            return lifetimeScope.BeginLifetimeScope(tag, builder =>
            {
                foreach (IServiceLoader loader in lifetimeScope.Resolve<IEnumerable<IServiceLoader>>())
                {
                    if (loader.LifetimeScopeTag.Equals(tag))
                    {
                        loader.ConfigureServices(builder);
                    }
                }

                build?.Invoke(builder);
            });
        }

        public static bool HasTag(this ILifetimeScope lifetimeScope, object tag)
        {
            return lifetimeScope.Resolve<ITags>().Has(tag);
        }

        public static bool IsTag(this ILifetimeScope lifetimeScope, object tag)
        {
            return lifetimeScope.Tag.Equals(tag);
        }

        public static bool StateMatches(this ILifetimeScope lifetimeScope, object state)
        {
            return lifetimeScope.Resolve<IStateProvider>().Matches(state);
        }
    }
}
