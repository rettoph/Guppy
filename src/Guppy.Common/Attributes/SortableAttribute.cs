using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = true)]
    public class SortableAttribute : Attribute
    {
        public readonly Type Type;
        public readonly int Order;

        public SortableAttribute(Type type, int order)
        {
            this.Type = type;
            this.Order = order;
        }

        public bool Sorts(Type enumerableType)
        {
            return this.Type.IsAssignableTo(enumerableType);
        }
    }

    public class SortableAttribute<T> : SortableAttribute
    {
        public SortableAttribute(int order) : base(typeof(T), order)
        {
        }
    }
}
