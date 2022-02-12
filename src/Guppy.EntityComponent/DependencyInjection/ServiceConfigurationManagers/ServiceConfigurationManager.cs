using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public abstract class ServiceConfigurationManager : IDisposable
    {
        /// <summary>
        /// Get an instance of the managed service configuration.
        /// </summary>
        /// <returns></returns>
        public abstract Object GetInstance();

        /// <summary>
        /// Get an instance of the managed service configuration.
        /// </summary>
        /// <returns></returns>
        public abstract Object GetInstance(Action<Object, ServiceProvider, ServiceConfiguration> customSetup, Int32 customSetupOrder);

        /// <summary>
        /// Build a new instance of the contained service reguardless of its lifetime.
        /// </summary>
        /// <returns></returns>
        public abstract Object BuildInstance();

        /// <summary>
        /// Build a new instance of the contained service reguardless of its lifetime.
        /// </summary>
        /// <returns></returns>
        public abstract Object BuildInstance(Action<Object, ServiceProvider, ServiceConfiguration> customSetup, Int32 customSetupOrder);

        #region IDisposable Implementation
        public virtual void Dispose()
        {
            //
        }
        #endregion
    }
}
