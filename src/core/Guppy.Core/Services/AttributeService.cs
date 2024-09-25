using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Core.Common.Services;
using System.Collections;

namespace Guppy.Core.Services
{
    internal sealed class AttributeService<TType, TAttribute> : IAttributeService<TType, TAttribute>
        where TAttribute : Attribute
    {
        private Dictionary<Type, TAttribute[]> _attributes;

        public ITypeService<TType> Types { get; }

        public TAttribute[] this[Type type] => _attributes[type];

        public AttributeService(ITypeService<TType> types, bool inherit)
        {
            Types = types;

            _attributes = new Dictionary<Type, TAttribute[]>(Types.Count());

            foreach (Type type in Types)
            {
                _attributes.Add(type, type.GetAllCustomAttributes<TAttribute>(inherit).ToArray());
            }
        }

        public TAttribute[] Get<T>()
            where T : TType
        {
            return _attributes[typeof(T)];
        }

        public IEnumerator<(Type, TAttribute[])> GetEnumerator()
        {
            foreach ((Type type, TAttribute[] attributes) in _attributes)
            {
                yield return (type, attributes);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
