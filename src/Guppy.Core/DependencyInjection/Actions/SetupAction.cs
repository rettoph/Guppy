using Guppy.DependencyInjection.Interfaces;
using Guppy.DependencyInjection.ServiceConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Actions
{
    /// <summary>
    /// Simple container for an action to be ran when initializing
    /// a new service instance. These will be linked by an internal
    /// <see cref="ServiceConfigurationKey"/> value & automatically
    /// applied to all inheriting services.
    /// </summary>
    public class SetupAction : BaseAction<ServiceConfigurationKey, IServiceConfiguration>
    {
        public SetupAction(
            ServiceConfigurationKey key, 
            Action<Object, GuppyServiceProvider, IServiceConfiguration> method,
            Int32 order = 0,
            Func<IAction<ServiceConfigurationKey, IServiceConfiguration>, ServiceConfigurationKey, Boolean> filter = default) : base(key, method, order, filter)
        {
        }

        public override bool Filter(ServiceConfigurationKey key)
        {
            return key.Inherits(this.Key) && base.Filter(key);
        }
    }
}
