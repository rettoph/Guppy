using Guppy.Core.Common.Services;

namespace Guppy.Core.Common.Extensions
{
    public static class ISystemServiceExtensions
    {
        public static IEnumerable<T> GetAll<T>(this IScopedSystemService systemService)
        {
            return systemService.GetAll().OfType<T>();
        }

        public static IEnumerable<T> GetAll<T>(this IGlobalSystemService systemService)
        {
            return systemService.GetAll().OfType<T>();
        }
    }
}
