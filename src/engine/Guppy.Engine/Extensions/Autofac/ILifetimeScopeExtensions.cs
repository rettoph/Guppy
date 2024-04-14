using Autofac;
using Guppy.Engine.Common;
using Guppy.Engine.Loaders;

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
                    var loaders = lifetimeScope.Resolve<IEnumerable<IServiceLoader>>();

                    foreach (IServiceLoader loader in loaders)
                    {
                        try
                        {
                            if (loader.LifetimeScopeTag.Equals(tag))
                            {
                                loader.ConfigureServices(builder);
                            }
                        }
                        catch (Exception ex)
                        {
                            GuppyLogger.LogException($"{nameof(ILifetimeScopeExtensions)}::{nameof(BeginGuppyScope)} -  Exception calling {loader.GetType().GetFormattedName()}{nameof(IServiceLoader.ConfigureServices)}, Tag = {tag}", ex);
                        }
                    }

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
    }
}
