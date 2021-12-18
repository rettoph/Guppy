using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public abstract class TypeFactory
    {
        public readonly Type Type;

        protected TypeFactory(Type type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Return a configured instance of the defined type.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public abstract void BuildInstance(
            ServiceProvider provider, 
            ServiceConfiguration configuration,
            out Object instance);

        /// <summary>
        /// Return a configured instance of the defined type.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="configuration"></param>
        /// <param name="customSetup"></param>
        /// <param name="customSetupOrder"></param>
        /// <returns></returns>
        public abstract void BuildInstance(
            ServiceProvider provider,
            ServiceConfiguration configuration,
            Action<Object, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder,
            out Object instance);

        /// <summary>
        /// Attempt to return a given item back into the factory pool
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public abstract Boolean TryReturnToPool(Object instance);
    }

    public sealed class TypeFactory<TFactory> : TypeFactory
        where TFactory : class
    {
        #region Private Fields
        private Pool<TFactory> _pool;
        #endregion

        #region Public Fields

        public readonly Func<ServiceProvider, TFactory> Method;
        public readonly CustomAction<TypeFactory, ITypeFactoryBuilder>[] Builders;
        public UInt16 MaxPoolSize;
        #endregion

        #region Constructor
        public TypeFactory(
            Type type, 
            Func<ServiceProvider, TFactory> method,
            CustomAction<TypeFactory, ITypeFactoryBuilder>[] builders,
            ushort maxPoolSize) : base(type)
        {
            this.Method = method;
            this.Builders = builders;
            this.MaxPoolSize = maxPoolSize;

            _pool = new Pool<TFactory>(ref this.MaxPoolSize);
        }
        #endregion

        #region Helper Methods
        private void GetInstance(ServiceProvider provider, out TFactory instance)
        {
            if (!_pool.TryPull(out instance))
            {
                instance = this.Method(provider);

                foreach (CustomAction<TypeFactory, ITypeFactoryBuilder> builder in this.Builders)
                {
                    builder.Invoke(instance, provider, this);
                }
            }
        }
        #endregion

        #region TypeFactory Implementation
        /// <inheritdoc />
        public override void BuildInstance(ServiceProvider provider, ServiceConfiguration configuration, out Object instance)
        {
            this.GetInstance(provider, out TFactory item);
            instance = item;

            foreach (CustomAction<ServiceConfiguration, IServiceConfigurationBuilder> setup in configuration.Setups)
            {
                setup.Invoke(item, provider, configuration);
            }
        }

        /// <inheritdoc />
        public override void BuildInstance(
            ServiceProvider provider,
            ServiceConfiguration configuration,
            Action<Object, ServiceProvider, ServiceConfiguration> customSetup,
            Int32 customSetupOrder,
            out Object instance)
        {
            this.GetInstance(provider, out TFactory item);
            instance = item;

            Boolean ranCustomSetup = false;
            foreach (CustomAction<ServiceConfiguration, IServiceConfigurationBuilder> setup in configuration.Setups)
            {
                if (!ranCustomSetup && setup.Order >= customSetupOrder)
                {
                    customSetup(item, provider, configuration);
                    ranCustomSetup = true;
                }

                setup.Invoke(item, provider, configuration);
            }

            if (!ranCustomSetup)
            {
                customSetup(item, provider, configuration);
            }
        }

        /// <inheritdoc />
        public override bool TryReturnToPool(Object instance)
        {
            if(instance is TFactory item)
            {
                return _pool.TryReturn(item);
            }

            return false;
        }
        #endregion
    }
}
