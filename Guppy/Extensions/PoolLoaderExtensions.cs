using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Implementations;
using Guppy.Loaders;
using Guppy.Utilities.Pools;

namespace Guppy.Extensions
{
    public static class PoolLoaderExtensions
    {
        public static void TryRegisterInitializable<T>(this PoolLoader loader, UInt16 priority = 100)
            where T : Initializable
        {
            loader.TryRegisterInitializable(
                typeof(T),
                priority);
        }
        public static void TryRegisterInitializable(this PoolLoader loader, Type targetType, UInt16 priority = 100)
        {
            if (!typeof(Initializable).IsAssignableFrom(targetType))
                throw new Exception($"Unable to register pool. Target Type<{targetType.Name}> does not extend Initializable.");

            loader.TryRegister(
                targetType,
                typeof(InitializablePool),
                priority);
        }
    }
}
