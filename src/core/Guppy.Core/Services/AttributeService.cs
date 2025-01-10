using System.Collections;
using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    internal sealed class AttributeService<TType, TAttribute> : IAttributeService<TType, TAttribute>
        where TAttribute : Attribute
    {
        private readonly Dictionary<Type, TAttribute[]> _attributes;

        public ITypeService<TType> Types { get; }

        public TAttribute[] this[Type type] => this._attributes[type];

        public AttributeService(ITypeService<TType> types, bool inherit)
        {
            this.Types = types;

            this._attributes = new Dictionary<Type, TAttribute[]>(this.Types.Count());

            foreach (Type type in this.Types)
            {
                this._attributes.Add(type, type.GetAllCustomAttributes<TAttribute>(inherit).ToArray());
            }
        }

        public TAttribute[] Get<T>()
            where T : TType
        {
            return this._attributes[typeof(T)];
        }

        public IEnumerator<(Type, TAttribute[])> GetEnumerator()
        {
            foreach ((Type type, TAttribute[] attributes) in this._attributes)
            {
                yield return (type, attributes);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}