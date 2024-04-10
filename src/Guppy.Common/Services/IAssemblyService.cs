using System.Reflection;

namespace Guppy.Common.Services
{
    public interface IAssemblyService : IEnumerable<Assembly>
    {
        AssemblyName[] Libraries { get; }

        event OnEventDelegate<IAssemblyService, Assembly>? OnAssemblyLoaded;

        /// <summary>
        /// Load the given <paramref name="assembly"/> if it references any of the required
        /// <see cref="Libraries"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="forced"></param>
        void Load(Assembly assembly, bool forced = false);

        /// <summary>
        /// Create a <see cref="ITypeService{T}"/> for the requested
        /// <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ITypeService<T> GetTypes<T>();

        /// <summary>
        /// Create a <see cref="ITypeService{T}"/> for the requested
        /// <typeparamref name="T"/> type so long as the <see cref="Type"/>.
        /// matches the recieved <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ITypeService<T> GetTypes<T>(Func<Type, bool> predicate);

        /// <summary>
        /// Return a cached table of all the <typeparamref name="TType"/> instances
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        IAttributeService<TType, TAttribute> GetAttributes<TType, TAttribute>(bool inherit)
            where TAttribute : Attribute;

        /// <summary>
        /// Return a cached table of all the <typeparamref name="TType"/> instances
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IAttributeService<TType, TAttribute> GetAttributes<TType, TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute;

        /// <summary>
        /// Return a cached table of all types
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        IAttributeService<object, TAttribute> GetAttributes<TAttribute>(bool inherit)
            where TAttribute : Attribute;

        /// <summary>
        /// Return a cached table of all types
        /// that contain a <typeparamref name="TAttribute"/> value.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IAttributeService<object, TAttribute> GetAttributes<TAttribute>(Func<Type, bool> predicate, bool inherit)
            where TAttribute : Attribute;
    }
}
