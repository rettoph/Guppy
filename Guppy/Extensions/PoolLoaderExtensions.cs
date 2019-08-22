using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Implementations;
using Guppy.Utilities;
using Guppy.Utilities.Loaders;
using Guppy.Utilities.Pools;

namespace Guppy.Extensions
{
    public static class PoolLoaderExtensions
    {
        #region Layer Methods
        public static void TryRegisterLayer<TLayer>(this PoolLoader loader, UInt16 priority = 100)
            where TLayer : Layer
        {
            loader.TryRegisterLayer(typeof(TLayer), priority);
        }
        public static void TryRegisterLayer(this PoolLoader loader, Type layerType, UInt16 priority = 100)
        {
            ExceptionHelper.ValidateAssignableFrom<Layer>(layerType);

            loader.TryRegisterInitializable(layerType, priority);
        }
        #endregion

        #region Initializable Methods
        public static void TryRegisterInitializable<T>(this PoolLoader loader, UInt16 priority = 100)
            where T : Initializable
        {
            loader.TryRegisterInitializable(
                typeof(T),
                priority);
        }
        public static void TryRegisterInitializable(this PoolLoader loader, Type targetType, UInt16 priority = 100)
        {
            ExceptionHelper.ValidateAssignableFrom<Initializable>(targetType);

            loader.TryRegister(
                targetType,
                typeof(InitializablePool),
                priority);
        }
        #endregion
    }
}
