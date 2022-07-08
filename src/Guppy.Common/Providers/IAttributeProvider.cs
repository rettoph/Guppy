using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface IAttributeProvider<TType, TAttribute> : IEnumerable<TAttribute>
        where TAttribute : Attribute
    {
        TAttribute this[Type type] { get; }
        ITypeProvider<TType> Types { get; }
        TAttribute Get<T>()
            where T : TType;
    }
}
