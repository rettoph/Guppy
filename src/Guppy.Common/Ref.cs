using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public class Ref
    {
        internal Ref()
        {

        }
    }
    public class Ref<T> : Ref
    {
        public T Value;

        public Ref(T value)
        {
            Value = value;
        }

        public static implicit operator T(Ref<T> @ref)
        {
            return @ref.Value;
        }
    }
}
