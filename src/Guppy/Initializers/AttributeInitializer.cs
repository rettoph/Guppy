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
    [AutoLoad]
    internal sealed class AttributeInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var typesAttributes = assemblies.GetAttributes<InitializableAttribute>(x =>
            {
                if(x.IsAbstract)
                {
                    return false;
                }

                if(x.IsInterface)
                {
                    return false;
                }

                return x.IsClass;
            }, true);

            foreach((Type type, InitializableAttribute[] attributes) in typesAttributes)
            {
                foreach(var attribute in attributes)
                {
                    attribute.Initialize(services, type);
                }
            }
        }
    }
}
