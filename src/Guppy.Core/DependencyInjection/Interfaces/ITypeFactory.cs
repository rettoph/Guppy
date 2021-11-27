using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Builders;
using Guppy.DependencyInjection.ServiceConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Interfaces
{
    public interface ITypeFactory
    {
        #region Properties
        /// <summary>
        /// The "lookup" type for the described factory.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The type to be passed into the internal
        /// <see cref="Method"/> when constructing a
        /// new instance. By default, this will be
        /// <see cref="Type"/>. If neccessary, a constructed
        /// type should be built.
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// List of actions to preform on an instance 
        /// before returning it.
        /// </summary>
        BuilderAction[] BuilderActions { get; }

        /// <summary>
        /// The maximum size of the factory's internal pool.
        /// </summary>
        UInt16 MaxPoolSize { get; set; }
        #endregion

        #region Methods 
        /// <summary>
        /// Attempt to return a TypeFactory instance back into 
        /// its relevant pool.
        /// </summary>
        /// <param name="instance"></param>
        Boolean TryReturnToPool(Object instance);

        /// <summary>
        /// Construct a new instance of current type. Optionally use
        /// recieved type arguments.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="generics"></param>
        /// <param name="cacher"></param>
        /// <returns></returns>
        Object BuildInstance(GuppyServiceProvider provider, Type[] generics);

        /// <summary>
        /// Mutate some input keys to fulfil the generic type, if needed.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        IEnumerable<ServiceConfigurationKey> MutateKeys(ServiceConfigurationKey[] keys, Type[] generics);

        /// <summary>
        /// Mutate a single input keys to fulfil the generic type, if needed.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        ServiceConfigurationKey MutateKey(ServiceConfigurationKey key, Type[] generics);
        #endregion
    }
}
