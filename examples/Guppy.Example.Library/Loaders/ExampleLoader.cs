using Guppy.Attributes;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Loaders
{
    [AutoLoad]
    internal sealed class ExampleLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGuppy<ExampleGuppy>();

            services.AddResourcePack("default", "Content/Default");

            services.AddStringResource("test");
            services.AddColorResource("ShipHull");
        }
    }
}
