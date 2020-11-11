using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Descriptors
{
    /// <summary>
    /// Simple placeholder class that will store registered factory
    /// information in a temporary setting. This will eventually be
    /// utilized in creating a more permanent ServiceFactory instance.
    /// </summary>
    public class ServiceFactoryDescriptor
    {
        #region Public Fields
        /// <summary>
        /// The "lookup" type for the described factory.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// An optional type utilized for generating which 
        /// ServiceSetups apply to the current service.
        /// 
        /// When null, the ServiceType will be used instead.
        /// </summary>

        public readonly Type ImplementationType;

        /// <summary>
        /// The factory method to create a brand new instance
        /// when requested.
        /// </summary>
        public readonly Func<ServiceProvider, Object> Factory;

        /// <summary>
        /// The priority value for this specific descriptor.
        /// Only the highest priority for each index will be 
        /// saved and used within the ServiceProvider.
        /// </summary>
        public readonly Int32 Priority;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new ServiceFactoryDescriptor instance.
        /// </summary>
        /// <param name="type">The "lookup" type for this service.</param>
        /// <param name="factory">The factory method to create a brand new instance when requested.</param>
        /// <param name="implementationType">An optional type usilized for generating which ServiceSetups apply to the current service. When null, the ServiceType will be used instead.</param>
        public ServiceFactoryDescriptor(
            Type type,
            Func<ServiceProvider, Object> factory,
            Type implementationType = null,
            int priotity = 0)
        {
            this.Type = type;
            this.ImplementationType = implementationType ?? this.Type;
            this.Factory = factory;
            this.Priority = priotity;
        }
        #endregion

    }
}
