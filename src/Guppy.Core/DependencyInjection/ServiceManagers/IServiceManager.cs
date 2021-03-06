﻿using Guppy.DependencyInjection.ServiceConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.ServiceManagers
{
    public interface IServiceManager : IDisposable
    {
        /// <summary>
        /// Return a service instance for the currently
        /// described ServiceConfiguration.
        /// </summary>
        /// <returns></returns>
        Object GetInstance();

        /// <summary>
        /// Return a service instance for the currently
        /// described ServiceConfiguration and preform
        /// a custom setup action if a new instance
        /// is initialized.
        /// </summary>
        /// <returns></returns>
        Object GetInstance(Action<Object, ServiceProvider, IServiceConfiguration> setup, Int32 setupOrder);
    }
}
