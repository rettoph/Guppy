﻿using System.Reflection;
using Guppy.Core.Serialization.Common.Factories;

namespace Guppy.Core.Serialization.Factories
{
    internal sealed class DefaultEnumerableFactory : IDefaultInstanceFactory
    {
        private static readonly MethodInfo _methodInfo = typeof(Enumerable).GetMethod(nameof(Enumerable.Empty), BindingFlags.Public | BindingFlags.Static) ?? throw new NotImplementedException();
        public bool CanConstructType(Type type)
        {
            if (type.IsConstructedGenericType == false)
            {
                return false;
            }

            Type definition = type.GetGenericTypeDefinition();
            return definition == typeof(IEnumerable<>);
        }

        public object BuildInstance(Type type)
        {
            object? result = _methodInfo.MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(null, []);

            return result ?? throw new NotImplementedException();
        }
    }
}