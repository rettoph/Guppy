using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Lists.Interfaces
{
    /// <summary>
    /// Simple interface used to auto add factory
    /// functionality into a service list.
    /// 
    /// Simply imlement this service then 
    /// access to help extension methods will be
    /// available.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IFactoryServiceList : IServiceList
    {
        /// <summary>
        /// Primary provider used for internal factory creation.
        /// </summary>
        ServiceProvider Provider { get; }
    }
}
