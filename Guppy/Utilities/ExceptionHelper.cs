﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    internal static class ExceptionHelper
    {
        public static void ValidateAssignableFrom(Type baseType, Type targetType)
        {
            if (!baseType.IsAssignableFrom(targetType))
                throw new ArgumentException($"Unable to assign Type<{targetType.Name}> to Type<{baseType}>.");
        }

        /// <summary>
        /// Verify that the input type is assignable from the given base type.
        /// 
        /// If it is not, throw ann exception.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="targetType"></param>
        public static void ValidateAssignableFrom<TBase>(Type targetType)
        {
            ExceptionHelper.ValidateAssignableFrom(typeof(TBase), targetType);
        }
    }
}