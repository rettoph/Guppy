using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Common;

namespace Guppy.Attributes
{
    public class WithFilterAttribute : Attribute
    {
        public readonly Type Type;

        public WithFilterAttribute(Type type)
        {
            ThrowIf.Type.IsNotAssignableFrom<IFilter>(type);

            this.Type = type;
        }

        /// <summary>
        /// Return all <see cref="WithFilterAttribute.Type"/> values on inherited attributes
        /// attached to the recieved <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes(Type type)
        {
            return type.GetCustomAttributes(typeof(WithFilterAttribute), true).Select(x => ((WithFilterAttribute)x).Type);
        }

        /// <summary>
        /// Return all <see cref="WithFilterAttribute.Type"/> values on inherited attributes
        /// attached to the recieved <typeparamref name="T"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes<T>()
        {
            return WithFilterAttribute.GetTypes(typeof(T));
        }
    }
}
