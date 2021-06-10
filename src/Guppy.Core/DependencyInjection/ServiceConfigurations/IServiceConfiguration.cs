using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.ServiceManagers;
using Guppy.DependencyInjection.TypeFactories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.ServiceConfigurations
{
    public interface IServiceConfiguration
    {
        #region Properties
        /// <summary>
        /// The primary lookup key for the
        /// current service configuration.
        /// </summary>
        ServiceConfigurationKey Key { get; }

        /// <summary>
        /// The default lifetime for the described service.
        /// </summary>
        ServiceLifetime Lifetime { get; }

        /// <summary>
        /// The <see cref="ITypeFactory.Type"/> to be used when building a 
        /// new instance of this service.
        /// </summary>
        ITypeFactory TypeFactory { get; }

        /// <summary>
        /// A list of actions to preform on services when initializing
        /// a new instance.
        /// </summary>
        SetupAction[] SetupActions { get; }

        /// <summary>
        /// When a non <see cref="ServiceLifetime.Transient"/> service gets created
        /// the value will be linked within the defined cache keys so that any of
        /// the values will return the currently defined service.
        /// </summary>
        ServiceConfigurationKey[] DefaultCacheKeys { get; }
        #endregion

        #region Methods
        IServiceManager BuildServiceManager(ServiceProvider provider, Type[] generics);
        #endregion
    }

    public static class IServiceConfigurationExtensions
    {
        public static void BuildInstance(
            this IServiceConfiguration configuration, 
            ServiceProvider provider, 
            Type[] generics, 
            out Object instance)
        {
            instance = configuration.TypeFactory.BuildInstance(provider, generics);

            foreach (SetupAction action in configuration.SetupActions)
                action.Invoke(instance, provider, configuration);
        }
        public static Object BuildInstance(
            this IServiceConfiguration configuration, 
            ServiceProvider provider, 
            Type[] generics)
        {
            Object instance;

            configuration.BuildInstance(provider, generics, out instance);

            return instance;
        }

        public static void BuildInstance(
            this IServiceConfiguration configuration,
            ServiceProvider provider,
            Type[] generics,
            Action<Object, ServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder,
            out Object instance)
        {
            instance = configuration.TypeFactory.BuildInstance(provider.Root, generics);
            Boolean ranSetup = false;

            foreach (SetupAction action in configuration.SetupActions)
            {
                if(!ranSetup && action.Order >= setupOrder)
                {
                    setup(instance, provider, configuration);
                    ranSetup = true;
                }

                action.Invoke(instance, provider, configuration);
            }

            if (!ranSetup)
                setup(instance, provider, configuration);
        }
        public static Object BuildInstance(
            this IServiceConfiguration configuration,
            ServiceProvider provider,
            Type[] generics,
            Action<Object, ServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder)
        {
            Object instance;

            configuration.BuildInstance(provider, generics, setup, setupOrder, out instance);

            return instance;
        }

        public static Object GetInstance(
            this IServiceConfiguration configuration, 
            ServiceProvider provider, 
            Type[] generics)
                => provider.GetServiceManager(configuration, generics).GetInstance();

        public static Object GetInstance(
            this IServiceConfiguration configuration, 
            ServiceProvider provider, 
            Type[] generics, 
            Action<Object, ServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder)
                => provider.GetServiceManager(configuration, generics).GetInstance(setup, setupOrder);
    }
}
