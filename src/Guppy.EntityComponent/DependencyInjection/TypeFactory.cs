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
    }

    public sealed class TypeFactory<TFactory> : TypeFactory
        where TFactory : class
    {
        #region Public Fields

        public readonly Func<ServiceProvider, TFactory> Method;
        #endregion

        #region Constructor
        public TypeFactory(
            Type type, 
            Func<ServiceProvider, TFactory> method) : base(type)
        {
            this.Method = method;
        }
        #endregion

        #region TypeFactory Implementation
        /// <inheritdoc />
        public override void BuildInstance(ServiceProvider provider, ServiceConfiguration configuration, out Object instance)
        {
            instance = this.Method(provider);

            foreach (CustomAction<ServiceConfiguration, IServiceConfigurationBuilder> setup in configuration.Setups)
            {
                setup.Invoke(instance, provider, configuration);
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
            instance = this.Method(provider);

            Boolean ranCustomSetup = false;
            foreach (CustomAction<ServiceConfiguration, IServiceConfigurationBuilder> setup in configuration.Setups)
            {
                if (!ranCustomSetup && setup.Order >= customSetupOrder)
                {
                    customSetup(instance, provider, configuration);
                    ranCustomSetup = true;
                }

                setup.Invoke(instance, provider, configuration);
            }

            if (!ranCustomSetup)
            {
                customSetup(instance, provider, configuration);
            }
        }
        #endregion
    }
}
