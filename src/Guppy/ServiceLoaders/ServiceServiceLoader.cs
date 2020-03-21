using Guppy.Attributes;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Simple service provider
    /// useful for auto registering a service
    /// without requiring a service provider.
    /// 
    /// This will do it automatically.
    /// 
    /// Just add the AutoLoad attribute to a class
    /// implementing the IService interface
    /// </summary>
    [AutoLoad]
    internal sealed class ServiceServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            AssemblyHelper.GetTypesWithAutoLoadAttribute<IService>(false).ForEach(t =>
            {
                t.GetCustomAttributes(typeof(ServiceAttribute), false).Select(attr => attr as ServiceAttribute).OrderBy(attr => attr.Priority).ForEach(attr =>
                {
                    services.Add(new ServiceDescriptor(
                        handle: attr.Handle ?? t.FullName,
                        lifetime: attr.Lifetime,
                        type: t,
                        baseType: attr.BaseType ?? t,
                        setup: null));
                });
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
