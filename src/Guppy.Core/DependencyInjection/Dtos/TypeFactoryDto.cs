using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.TypeFactories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Dtos

{
    public struct TypeFactoryDto
    {
        #region Public Fields
        /// <summary>
        /// The "lookup" type for the described factory.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// The type to be passed into the internal
        /// <see cref="Method"/> when constructing a
        /// new instance. By default, this will be
        /// <see cref="Type"/>. If neccessary, a constructed
        /// generic type should be built.
        /// </summary>
        public readonly Type ImplementationType;

        /// <summary>
        /// The factory method to create a brand new instance
        /// when requested.
        /// </summary>
        public Func<GuppyServiceProvider, Type, Object> Method;

        /// <summary>
        /// The maximum size of the factory's internal pool.
        /// </summary>
        public readonly UInt16 MaxPoolSize;

        /// <summary>
        /// The priority value for this specific descriptor.
        /// All factories will be sorted by priority building
        /// the provider.
        /// </summary>
        public readonly Int32 Priority;
        #endregion

        #region Constructors
        public TypeFactoryDto(
            Type type,
            Type typeImplementation,
            Func<GuppyServiceProvider, Type, Object> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
        {
            this.Type = type;
            this.ImplementationType = typeImplementation;
            this.Method = method;
            this.MaxPoolSize = maxPoolSize;
            this.Priority = priority;
        }
        public TypeFactoryDto(
            Type type,
            Func<GuppyServiceProvider, Type, Object> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0) : this(type, type, method, maxPoolSize, priority)
        {
        }
        #endregion

        #region Helper Methods
        internal ITypeFactory CreateTypeFactory(IEnumerable<BuilderAction> builders)
        {
            if (this.ImplementationType.IsGenericTypeDefinition)
                return new GenericTypeFactory(this, builders);

            return new TypeFactory(this, builders);
        }
        #endregion
    }
}
