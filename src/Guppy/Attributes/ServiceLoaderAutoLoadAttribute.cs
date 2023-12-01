using Autofac;
using Guppy.Extensions.Autofac;
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
        protected override void Configure(ContainerBuilder builder, Type classType)
        {
            builder.RegisterServiceLoader(classType);
        }
    }
}
