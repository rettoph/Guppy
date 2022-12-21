using Guppy.Attributes;
using Guppy.Common.Attributes;
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
            var typesInitializableAttributes = assemblies.GetAttributes<InitializableAttribute>(true);

            foreach((Type type, InitializableAttribute[] attributes) in typesInitializableAttributes)
            {
                foreach(var attribute in attributes)
                {
                    attribute.Initialize(services, type);
                }
            }
        }
    }
}
