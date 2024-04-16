using System.Reflection;

namespace Guppy.Core.Commands.Common.Attributes
{
    public abstract class FactoryAttribute<T> : Attribute
    {
        private int _memberHash;
        private T _instance = default!;

        internal T Get(MemberInfo member)
        {
            if (_memberHash != member.GetHashCode())
            {
                _instance = this.Build(member);
                _memberHash = member.GetHashCode();
            }

            return _instance;
        }

        protected abstract T Build(MemberInfo member);

        public static T[] GetAll(Type type)
        {
            List<T> output = new List<T>();
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                FactoryAttribute<T>? attribute = propertyInfo.GetCustomAttribute<FactoryAttribute<T>>(true);
                if (attribute is not null)
                {
                    output.Add(attribute.Get(propertyInfo));
                }
            }

            return output.ToArray();
        }
    }
}
