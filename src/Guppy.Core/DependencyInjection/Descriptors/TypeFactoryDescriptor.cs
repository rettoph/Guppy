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
    public class TypeFactoryDescriptor
    {
        #region Public Fields
        /// <summary>
        /// The "lookup" type for the described factory.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// The factory method to create a brand new instance
        /// when requested.
        /// </summary>
        public readonly Func<ServiceProvider, Type, Object> Method;

        /// <summary>
        /// The maximum size of the factories internal pool.
        /// </summary>
        public UInt16 MaxPoolSize;

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
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        public TypeFactoryDescriptor(
            Type type,
            Func<ServiceProvider, Type, Object> method,
            UInt16 maxPoolSize = 100,
            Int32 priority = 0)
        {
            this.Type = type;
            this.Method = method;
            this.MaxPoolSize = maxPoolSize;
            this.Priority = priority;
        }
        #endregion

    }
}
