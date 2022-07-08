using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface IAssemblyProvider : IEnumerable<Assembly>
    {
        AssemblyName[] Libraries { get; }

        event OnEventDelegate<IAssemblyProvider, Assembly>? OnAssemblyLoaded;

        /// <summary>
        /// Load the given <paramref name="assembly"/> if it references any of the required
        /// <see cref="Libraries"/>.
        /// </summary>
        /// <param name="assembly"></param>
        void Load(Assembly assembly);

        /// <summary>
        /// Create a <see cref="ITypeProvider{T}"/> for the requested
        /// <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ITypeProvider<T> GetTypes<T>();

        /// <summary>
        /// Create a <see cref="ITypeProvider{T}"/> for the requested
        /// <typeparamref name="T"/> type so long as the <see cref="Type"/>.
        /// matches the recieved <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ITypeProvider<T> GetTypes<T>(Func<Type, bool> predicate);

        /// <summary>
        /// Return a cached table of all the <typeparamref name="TType"/> instances
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        IAttributeProvider<TType, TAttribute> GetAttributes<TType, TAttribute>()
            where TAttribute : Attribute;

        /// <summary>
        /// Return a cached table of all the <typeparamref name="TType"/> instances
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IAttributeProvider<TType, TAttribute> GetAttributes<TType, TAttribute>(Func<Type, bool> predicate)
            where TAttribute : Attribute;

        /// <summary>
        /// Return a cached table of all types
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        IAttributeProvider<object, TAttribute> GetAttributes<TAttribute>()
            where TAttribute : Attribute;

        /// <summary>
        /// Return a cached table of all types
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IAttributeProvider<object, TAttribute> GetAttributes<TAttribute>(Func<Type, bool> predicate)
            where TAttribute : Attribute;
    }
}
