using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Common;

namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class FactoryAttribute : Attribute
    {
        public readonly Type Type;

        public FactoryAttribute(Type type)
        {
            this.Type = type;
        }

        public T GetInstance<T>(Type classType)
        {
            ThrowIf.Type.IsNotAssignableFrom<T>(this.Type);

            var instance = (T)this.GetInstance(typeof(T), classType);

            return instance;
        }

        protected abstract object GetInstance(Type serviceType, Type classType);

        /// <summary>
        /// Return all <see cref="FactoryAttribute.Type"/> values on inherited attributes
        /// attached to the recieved <paramref name="classType"/>.
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static IEnumerable<TOut> GetInstances<TOut>(Type classType)
        {
            var instances = classType.GetCustomAttributes(typeof(FactoryAttribute), true)
                .Select(x => (FactoryAttribute)x)
                .Where(x => x.Type.IsAssignableTo(typeof(TOut)))
                .Select(x => x.GetInstance<TOut>(classType));

            return instances;
        }

        /// <summary>
        /// Return all <see cref="FactoryAttribute.Type"/> values on inherited attributes
        /// attached to the recieved <typeparamref name="TIn"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<TOut> GetInstances<TIn, TOut>()
        {
            return FactoryAttribute.GetInstances<TOut>(typeof(TIn));
        }
    }
}
