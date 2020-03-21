using Guppy.Attributes;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LoaderServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            // throw new NotImplementedException();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            AssemblyHelper.GetTypesWithAutoLoadAttribute<ILoader>().Select(t => (ILoader)provider.GetService(t)).ForEach(l =>
            { // Load all auto loaded drivers now.
                l.Load(provider);
            });
        }
    }
}
