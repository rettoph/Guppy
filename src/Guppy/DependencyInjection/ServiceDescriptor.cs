using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// Simple class used to describe a specific service.
    /// Note: Services are indexed by thei
    /// </summary>
    internal sealed class ServiceDescriptor
    {
        #region Attributes
        /// <summary>
        /// A unique human readable identifier for the current
        /// service.
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// The internal factory used to generate a fresh instance of
        /// the current service.
        /// </summary>
        public readonly ServiceFactory Factory;

        /// <summary>
        /// The internally stored type used when the service lifetime
        /// is scoped or singleton. If null, then the cache type will
        /// default to the recieved factory service type.
        /// </summary>
        public readonly Type CacheType;
        #endregion

        #region Constructor
        public ServiceDescriptor(String name, ServiceFactory factory, Type cacheType)
        {
            this.Name = name;
            this.Factory = factory;
            this.CacheType = cacheType ?? null;
        }
        #endregion
    }
}
