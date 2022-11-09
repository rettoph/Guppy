using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Initializers
{
    [AutoLoad(int.MaxValue)]
    internal sealed class AttributeInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var typesAttributes = assemblies.GetAttributes<InitializableAttribute>(true);

            foreach((Type type, InitializableAttribute[] attributes) in typesAttributes)
            {
                foreach(var attribute in attributes)
                {
                    if(!attribute.ShouldInitialize(services, type))
                    {
                        continue;
                    }

                    attribute.Initialize(services, type);
                }
            }
        }
    }
}
