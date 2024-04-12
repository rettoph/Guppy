using Guppy.Attributes;
using System.Reflection;

namespace Guppy.Serialization.Factories
{
    [AutoLoad]
    internal sealed class DefaultEnumerableFactory : IDefaultInstanceFactory
    {
        private static MethodInfo _methodInfo = typeof(Enumerable).GetMethod(nameof(Enumerable.Empty), BindingFlags.Public | BindingFlags.Static) ?? throw new NotImplementedException();
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
            var result = _methodInfo.MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(null, Array.Empty<object>());

            return result ?? throw new NotImplementedException();
        }
    }
}
