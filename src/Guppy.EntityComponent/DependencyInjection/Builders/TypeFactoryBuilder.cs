using Minnow.General.Interfaces;
using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public class TypeFactoryBuilder<TFactory> : ITypeFactoryBuilder, IFluentPrioritizable<TypeFactoryBuilder<TFactory>>
        where TFactory : class
    {
        #region Private Fields
        private Func<ServiceProvider, TFactory> _method;
        private UInt16? _maxPoolSize;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public Type Type { get; }

        /// <inheritdoc />
        public Int32 Priority { get; set; }

        /// <summary>
        /// The primary factory method.
        /// </summary>
        public Func<ServiceProvider, TFactory> Method
        {
            get => _method;
            set => this.SetMethod(value);
        }

        /// <summary>
        /// The maximum size of the factory's internal pool.
        /// </summary>
        public UInt16? MaxPoolSize
        {
            get => _maxPoolSize;
            set => this.SetMaxPoolSize(value);
        }
        #endregion

        #region Constructors
        public TypeFactoryBuilder(
            Type type)
        {
            typeof(TFactory).ValidateAssignableFrom(type);

            this.Type = type;
        }
        #endregion

        #region SetMethod Methods
        /// <summary>
        /// Set the primary factory method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public TypeFactoryBuilder<TFactory> SetMethod(Func<ServiceProvider, TFactory> method)
        {
            _method = method;

            return this;
        }

        /// <summary>
        /// Set the primary factory method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public TypeFactoryBuilder<TFactory> SetMethod<TOut>(Func<ServiceProvider, TOut> method)
            where TOut : class, TFactory
        {
            _method = method;

            return this;
        }

        public TypeFactoryBuilder<TFactory> SetDefaultConstructor<TOut>()
            where TOut : class, TFactory, new()
        {
            return this.SetMethod(_ => new TOut());
        }
        #endregion

        #region SetMaxPoolSize Methods
        /// <summary>
        /// Set the maximum size of the factory's internal pool.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public TypeFactoryBuilder<TFactory> SetMaxPoolSize(UInt16? maxPoolSize)
        {
            _maxPoolSize = maxPoolSize;

            return this;
        }
        #endregion

        #region TypeFactoryBuilder Implementation
        TypeFactory ITypeFactoryBuilder.Build(
            IEnumerable<CustomAction<TypeFactory, ITypeFactoryBuilder>> allBuilders)
        {
            TFactory DefaultMethod(ServiceProvider provider)
            {
                return Activator.CreateInstance(this.Type) as TFactory;
            }

            CustomAction<TypeFactory, ITypeFactoryBuilder>[] builders = allBuilders.Where(b => {
                return this.Type.IsAssignableToOrSubclassOfGenericDefinition(b.AssignableFactoryType)&& b.Filter(this);
            }).ToArray();

            return new TypeFactory<TFactory>(
                type: this.Type,
                method: this.Method ?? DefaultMethod,
                builders: builders,
                maxPoolSize: this.MaxPoolSize ?? 500);
        }
        #endregion
    }
}
