using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Verify that the base type is assignable from the given target type.
        /// 
        /// If it is not, throw an exception.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="targetType"></param>
        public static void ValidateAssignableFrom(Type baseType, Type targetType)
        {
            if (!baseType.IsAssignableFrom(targetType))
                throw new ArgumentException($"Unable to assign Type<{targetType.Name}> to Type<{baseType}>.");
        }

        /// <summary>
        /// Verify that the target type is assignable from the given base type.
        /// 
        /// If it is not, throw an exception.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="targetType"></param>
        public static void ValidateAssignableFrom<TBase>(Type targetType)
        {
            ExceptionHelper.ValidateAssignableFrom(typeof(TBase), targetType);
        }
    }
}
