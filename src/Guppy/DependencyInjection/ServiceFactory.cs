using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    internal sealed class ServiceFactory
    {
        #region Private Fields
        private Func<ServiceProvider, Object> _factory;
        #endregion

        #region Public Fields
        /// <summary>
        /// The base type returned by this factory instance.
        /// </summary>
        public readonly Type Type;
        #endregion

        #region Constructor
        internal ServiceFactory(Type type, Func<ServiceProvider, Object> factory, Int32 priority)
        {
            _factory = factory;

            this.Type = type;
        }
        #endregion

        #region Public Methods
        public Object Build(ServiceProvider provider)
            => _factory.Invoke(provider);
        #endregion
    }
}
