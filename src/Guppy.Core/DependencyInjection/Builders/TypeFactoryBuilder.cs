using DotNetUtils.General.Interfaces;
using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Interfaces;
using Guppy.DependencyInjection.TypeFactories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Builders

{
    public class TypeFactoryBuilder : IPrioritizable<TypeFactoryBuilder>
    {
        #region Private Fields
        private Type _implementationType;
        private UInt16 _maxPoolSize;
        #endregion

        #region Public Fields
        /// <summary>
        /// The "lookup" type for the described factory.
        /// Primary key.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// The factory method to create a brand new instance
        /// when requested.
        /// </summary>
        public readonly Func<GuppyServiceProvider, Type, Object> Method;
        #endregion

        #region Public Properties
        /// <summary>
        /// The type to be passed into the internal
        /// <see cref="Method"/> when constructing a
        /// new instance. By default, this will be
        /// <see cref="Type"/>. If neccessary, a constructed
        /// generic type will be built.
        /// </summary>
        public Type ImplementationType
        {
            get => _implementationType;
            set => this.SetImplementationType(value);
        }



        /// <summary>
        /// The maximum size of the factory's internal pool.
        /// </summary>
        public UInt16 MaxPoolSize
        {
            get => _maxPoolSize;
            set => this.SetMaxPoolSize(value);
        }

        /// <summary>
        /// The priority value for this specific descriptor.
        /// All factories will be sorted by priority building
        /// the provider.
        /// </summary>
        public Int32 Priority { get; set; }
        #endregion

        #region Constructors
        public TypeFactoryBuilder(
            Type type,
            Func<GuppyServiceProvider, Type, Object> method)
        {
            this.Type = type;
            this.Method = method;

            this.SetImplementationType(type)
                .SetMaxPoolSize(500);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Set the type to be passed into the internal
        /// <see cref="Method"/> when constructing a
        /// new instance. By default, this will be
        /// <see cref="Type"/>. If neccessary, a constructed
        /// generic type will be built.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public TypeFactoryBuilder SetImplementationType(Type implementationType)
        {
            this.Type.ValidateAssignableFrom(implementationType);

            _implementationType = implementationType;

            return this;
        }

        /// <summary>
        /// Set the maximum size of the factory's internal pool.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public TypeFactoryBuilder SetMaxPoolSize(UInt16 maxPoolSize)
        {
            _maxPoolSize = maxPoolSize;

            return this;
        }

        internal ITypeFactory Build(IEnumerable<BuilderAction> builders)
        {
            if (this.ImplementationType.IsGenericTypeDefinition)
                return new GenericTypeFactory(this, builders);

            return new TypeFactory(this, builders);
        }
        #endregion
    }
}
