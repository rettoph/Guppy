using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    internal sealed class AttributeProvider<TType, TAttribute> : IAttributeProvider<TType, TAttribute>
        where TAttribute : Attribute
    {
        private Dictionary<Type, TAttribute> _attributes;

        public ITypeProvider<TType> Types { get; }

        public TAttribute this[Type type] => _attributes[type];

        public AttributeProvider(ITypeProvider<TType> types) 
        {
            this.Types = types;

            _attributes = new Dictionary<Type, TAttribute>(this.Types.Count());

            foreach(Type type in this.Types)
            {
                _attributes.Add(type, type.GetCustomAttribute<TAttribute>() ?? throw new ArgumentException());
            }
        }

        public TAttribute Get<T>() 
            where T : TType
        {
            return _attributes[typeof(T)];
        }

        public IEnumerator<TAttribute> GetEnumerator()
        {
            return _attributes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
