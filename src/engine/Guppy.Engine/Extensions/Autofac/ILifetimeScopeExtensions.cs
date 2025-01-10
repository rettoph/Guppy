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
    }
}