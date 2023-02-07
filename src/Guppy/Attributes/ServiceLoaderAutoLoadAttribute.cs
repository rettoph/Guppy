using Guppy.Configurations;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    internal sealed class ServiceLoaderAutoLoadAttribute : AutoLoadingAttribute
    {
        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            var loader = Activator.CreateInstance(classType) as IServiceLoader;

            configuration.AddServiceLoader(loader ?? throw new NotImplementedException());
        }
    }
}
