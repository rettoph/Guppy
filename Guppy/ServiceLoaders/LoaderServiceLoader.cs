using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Guppy.Utilities.Pools;
using System.Linq;

namespace Guppy.ServiceLoaders
{
    public class LoaderServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            foreach (Type loaderType in AssemblyHelper.GetTypesWithAttribute<IsLoaderAttribute>())
            { // Iterate through all types that contain a loader attribute
                if (!typeof(ILoader).IsAssignableFrom(loaderType))
                    throw new Exception($"Invalid type with Loader attribute! {loaderType.Name}");

                // Register the loader type
                services.AddLoader(loaderType);
            }
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
