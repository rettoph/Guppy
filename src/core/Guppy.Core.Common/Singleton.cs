using Autofac;
using Guppy.Core.Common.Utilities;

namespace Guppy.Core.Common
{
    /// <summary>
    /// Provides no guarantees that the returned instance was actually registered as a singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Singleton<T>
        where T : notnull
    {
        public static readonly T Instance = StaticInstance<IContainer>.Value.Resolve<T>();
    }
}
